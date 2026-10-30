using Xunit;

// Schakel parallelle executie uit voor het hele project
[assembly: CollectionBehavior(DisableTestParallelization = true)]