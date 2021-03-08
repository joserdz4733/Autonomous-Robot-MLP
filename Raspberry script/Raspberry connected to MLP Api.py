import os
import serial
import base64
import json
import requests
import datetime
import time

ser = serial.Serial('/dev/ttyACM1', 9600)
command = 'fswebcam -r 640x480 --no-banner /home/pi/modulares/test.bmp'
NeuralNetworkId = "1e71a3d9-d08f-4529-92cb-882e47fd49b2"
url = "http://192.168.15.7:8080/mlp/api/neuralnetwork/"+NeuralNetworkId+"/ImageProcessingRasp"
encoding = "utf-8"
headers = {'Content-type':'application/json', 'Accept':'application/json'}

while 1:
    if(ser.in_waiting > 0):
        line = ser.readline().decode(encoding)
        ser.flushInput()
        ser.flushOutput()
        time.sleep(.1)
        if('Stopped' in line):
            line = ""
            os.system(command)
            file = open('/home/pi/modulares/test.bmp', 'rb')
            data = file.read()
            file.close()

            base64img = base64.b64encode(data)
            base64imgstr = base64img.decode()

            toMLP = {
                'ImageBase64WithMetadata' : base64imgstr
                }

            toMLPJson = json.dumps(toMLP)
            response = requests.post(url, data = toMLPJson, headers = headers)
            now = datetime.datetime.now()
            print("End", now)

            if(response.status_code == 200):
                responseJson = json.loads(response.text)
                print(response.text)
                print(responseJson["index"])
                print(responseJson["objectName"])
				if(responseJson["accuracy"] > 0.80):
					ser.write(responseJson["objectName"].encode())
