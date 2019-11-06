// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.


namespace Microsoft.BotBuilderSamples.Bots
{
    public class ConversationFlow
    {
        public Questions lastQuestion { get; set; }
        public enum Questions
        {
            None,
            Name,
            Age,
            Date
        }
    }
}