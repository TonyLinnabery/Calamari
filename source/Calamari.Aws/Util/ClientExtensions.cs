﻿using Amazon.CloudFormation;
using Amazon.Runtime;
using Amazon.S3;
using Calamari.Aws.Integration;

namespace Calamari.Aws.Util
{
    
    
    public static class ClientExtensions
    {
        public static TConfig AsClientConfig<TConfig>(this IAwsEnvironmentGeneration environment)
            where TConfig : ClientConfig, new()
        {
            return new TConfig()
            {
                RegionEndpoint = environment.AwsRegion,
                ProxyPort = environment.ProxyPort,
                ProxyCredentials = environment.ProxyCredentials,
                ProxyHost = environment.ProxyHost
            };
        }}

    public static class ClientHelpers
    {
        public static AmazonS3Client CreateS3Client(IAwsEnvironmentGeneration environment)
        {
            return new AmazonS3Client(environment.AwsCredentials, environment.AsClientConfig<AmazonS3Config>());
        }

        public static IAmazonCloudFormation CreateCloudFormationClient(IAwsEnvironmentGeneration environment)
        {
            return new AmazonCloudFormationClient(environment.AwsCredentials, 
                environment.AsClientConfig<AmazonCloudFormationConfig>());
        }
    }
}
