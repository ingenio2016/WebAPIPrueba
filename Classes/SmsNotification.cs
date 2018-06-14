using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebAPIPrueba.Classes
{
    public class SmsNotification
    {
        public object initializerService()
        {
            var sns = new AmazonSimpleNotificationServiceClient();

            // Create topic
            var topicArn = sns.CreateTopic(new CreateTopicRequest
            {
                Name = "SampleSNSTopic"
            }).TopicArn;
        }
    }
}