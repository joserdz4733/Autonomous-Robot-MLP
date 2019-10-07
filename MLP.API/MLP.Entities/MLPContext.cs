using Microsoft.EntityFrameworkCore;

namespace MLP.Entities
{
    public class MLPContext : DbContext
    {
        public MLPContext(DbContextOptions<MLPContext> options) : base(options)
        {
            //Database.Migrate();
        }

        public DbSet<NeuralNetwork> NeuralNetworks { get; set; }
        public DbSet<NeuralNetworkTrainingConfig> NeuralNetworkdTrainingConfigs { get; set; }
        public DbSet<Neuron> Neurons { get; set; }
        public DbSet<NeuronWeight> NeuronWeights { get; set; }
        public DbSet<PredictedObject> PredictedObjects { get; set; }
        public DbSet<ImageProcessingConfig> ImageProcessingConfigs { get; set; }

    }
}
