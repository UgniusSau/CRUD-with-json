global using Xunit;

// Need to turn off test parallelization so we can validate the run order
[assembly: CollectionBehavior(DisableTestParallelization = true)]

[CollectionDefinition("ShortageUtilsTests")]
public class Collection1 { }

[CollectionDefinition("UserUtilsTests")]
public class Collection2 { }

