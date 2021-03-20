using AutoMapper;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Data.Entities;

namespace MultiLayerPerceptron.Application.Profiles
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMappers();
        }

        private void CreateMappers()
        {
            CreateMap<NeuralNetwork, NeuralNetworkDto>()
                .ForMember(dest => dest.HiddenNeurons, opt => opt.MapFrom(src =>
                    src.TrainingConfig.HiddenNeuronElements))
                .ForMember(dest => dest.OutputNeurons, opt => opt.MapFrom(src =>
                    src.TrainingConfig.OutputNeuronElements)).ReverseMap();

            CreateMap<NeuralNetworkForCreationDto, NeuralNetwork>().ReverseMap();
            CreateMap<NeuralNetworkTrainingConfigForCreationDto, NeuralNetworkTrainingConfig>().ReverseMap();
            CreateMap<NeuronForCreationDto, Neuron>().ReverseMap();
            CreateMap<NeuronWeightForCreationDto, NeuronWeight>().ReverseMap();
            CreateMap<NeuronWeightForManipulation, NeuronWeight>().ReverseMap();
            CreateMap<PredictedObjectForCreationDto, PredictedObject>().ReverseMap();
            CreateMap<PredictedObject, PredictedObjectDto>().ReverseMap();

            CreateMap<Neuron, NeuronForManipulation>().ReverseMap();
            CreateMap<NeuralNetworkTrainingConfig, NeuralNetworkTrainingConfigDto>().ReverseMap();

            CreateMap<ImageProcessingConfig, ImageProcessingConfigDto>().ReverseMap();
            CreateMap<ImageProcessingConfigForCreationDto, ImageProcessingConfig>().ReverseMap();
            CreateMap<ImageProcessingConfigWithImageForCreationDto, ImageProcessingConfigForCreationDto>().ReverseMap();
            CreateMap<ImageProcessingConfigValuesDto, ImageProcessingConfigForCreationDto>().ReverseMap();
        }
    }
}
