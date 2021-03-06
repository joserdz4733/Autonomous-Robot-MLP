using MLP.Entities;
using Microsoft.EntityFrameworkCore;

namespace MultiLayerPerceptron.Data
{
    public class MlpContext : DbContext
    {
        public MlpContext(DbContextOptions<MlpContext> options) : base(options)
        {
        }

        public DbSet<NeuralNetwork> NeuralNetworks { get; set; }
        public DbSet<NeuralNetworkTrainingConfig> NeuralNetworkTrainingConfigs { get; set; }
        public DbSet<Neuron> Neurons { get; set; }
        public DbSet<NeuronWeight> NeuronWeights { get; set; }
        public DbSet<PredictedObject> PredictedObjects { get; set; }
        public DbSet<ImageProcessingConfig> ImageProcessingConfigs { get; set; }

    }
}
