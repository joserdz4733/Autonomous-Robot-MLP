using Microsoft.EntityFrameworkCore;
using MLP.Entities;
using MLP.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MLP.Services
{
    public class MLPRepository : IMLPRepository
    {
        private MLPContext _context;

        public MLPRepository(MLPContext context)
        {
            _context = context;
        }

        public void AddNeuralNetwork(NeuralNetwork neuralNetwork)
        {
            neuralNetwork.Id = Guid.NewGuid();
            _context.NeuralNetworks.Add(neuralNetwork);
        }

        public bool NeuralNetworkExists(Guid neuralNetworkId)
        {
            return _context.NeuralNetworks.AsNoTracking().Any(a => a.Id == neuralNetworkId);
        }

        public void DeleteNeuralNetwork(NeuralNetwork neuralNetwork)
        {
            _context.NeuralNetworks.Remove(neuralNetwork);
        }

        public NeuralNetwork GetNeuralNetwork(Guid neuralNetworkId)
        {
            return _context.NeuralNetworks.Include(a => a.TrainingConfig)
                //.Include("Neurons.Weights")
                .FirstOrDefault(a => a.Id == neuralNetworkId);
        }

        public NeuralNetwork GetFullNeuralNetwork(Guid neuralNetworkId)
        {
            return _context.NeuralNetworks
                .Include(a => a.TrainingConfig.PredictedObjects).AsNoTracking()
                .Include(a => a.Neurons).ThenInclude(a => a.Weights).AsNoTracking()
                .FirstOrDefault(a => a.Id == neuralNetworkId);
        }

        public IEnumerable<NeuralNetwork> GetNeuralNetworks()
        {
            return _context.NeuralNetworks
                .Include(a => a.TrainingConfig.PredictedObjects).AsNoTracking()
                .OrderBy(a => a.Name)
                .AsQueryable();
        }

        public IEnumerable<NeuralNetwork> GetNeuralNetworks(IEnumerable<Guid> neuralNetworkIds)
        {
            return _context.NeuralNetworks
                .Where(a => neuralNetworkIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();
        }

        public void UpdateNeuralNetwork(NeuralNetwork neuralNetwork)
        {
                _context.NeuralNetworks.Update(neuralNetwork);
        }

        public NeuralNetworkTrainingConfig GetNeuralNetworkTrainingConfig(Guid neuralNetworkId)
        {
            return _context.NeuralNetworkdTrainingConfigs
                //.OrderBy(a => a.PredictedObjects)
                .Include(a=> a.PredictedObjects)
                .FirstOrDefault(a => a.NeuralNetworkId == neuralNetworkId);
        }

        //image processing optimization
        public NeuralNetworkTrainingConfig GetTrainingConfig(Guid neuralNetworkId)
        {
            return _context.NeuralNetworkdTrainingConfigs
                .Include(a => a.PredictedObjects).AsNoTracking()
                .FirstOrDefault(a => a.NeuralNetworkId == neuralNetworkId);
        }

        public IList<Neuron> GetHiddenNeurons(Guid neuralNetworkId)
        {
            return _context.Neurons
            .Include(a => a.Weights).AsNoTracking()
            .Where(a => a.NeuralNetworkId == neuralNetworkId && a.NeuronType == NeuronType.Hidden)
            .OrderBy(a => a.Index).ToList();
        }

        public IList<Neuron> GetOutputNeurons(Guid neuralNetworkId)
        {
            return _context.Neurons
            .Include(a => a.Weights).AsNoTracking()
            .Where(a => a.NeuralNetworkId == neuralNetworkId && a.NeuronType == NeuronType.Output)
            .OrderBy(a => a.Index).ToList();
        }

        #region imageProcessingConfig
        public void AddImageProcessingConfig(ImageProcessingConfig imageProcessingConfig)
        {
            _context.ImageProcessingConfigs.Add(imageProcessingConfig);
        }

        public bool ImageProcessingConfigExists(int Id)
        {
            return _context.ImageProcessingConfigs.AsNoTracking().Any(a => a.Id == Id);
        }

        public void DeleteImageProcessingConfig(ImageProcessingConfig imageProcessingConfig)
        {
            _context.ImageProcessingConfigs.Remove(imageProcessingConfig);
        }

        public ImageProcessingConfig GetImageProcessingConfig(int Id)
        {
            return _context.ImageProcessingConfigs
                .FirstOrDefault(a => a.Id == Id);
        }

        public IEnumerable<ImageProcessingConfig> GetAllImageProcessingConfigs()
        {
            return _context.ImageProcessingConfigs
                .OrderBy(a => a.Active)
                .OrderBy(a => a.ConfigName)
                .ToList();
        }

        public ImageProcessingConfig GetActiveImageProcessingConfigByNeuralNetwork(Guid Id)
        {
            return _context.ImageProcessingConfigs
                .Where(a => a.NeuralNetworkId == Id && a.Active == true)
                .FirstOrDefault();

        }

        public IEnumerable<ImageProcessingConfig> GetImageProcessingConfigByNeuralNetwork(Guid Id)
        {
            return _context.ImageProcessingConfigs
                .Where(a => a.NeuralNetworkId == Id)
                .OrderBy(a => a.Active)
                .OrderBy(a => a.ConfigName)
                .ToList();

        }
        #endregion

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
