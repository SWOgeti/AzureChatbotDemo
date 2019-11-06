// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Bots
{

    public class EchoBot : ActivityHandler
    {
        private readonly BotState _userState;
        private readonly BotState _conversationState;

        public EchoBot (ConversationState conversationState, UserState userState)
        {
            _conversationState = conversationState;
            _userState = userState;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationFlow>(nameof(ConversationFlow));
            var flow = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationFlow());
            var userStateAccessors = _userState.CreateProperty<UserProfile>("User");
            var profile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());
            if (string.IsNullOrEmpty(profile.name))
            {
                await FillOutUserProfileAsync(flow, profile, turnContext);
            }
            await _conversationState.SaveChangesAsync(turnContext);
            await _userState.SaveChangesAsync(turnContext);
            await turnContext.SendActivityAsync(MessageFactory.Text($"Echo: {turnContext.Activity.Text}"), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello! How may I assist you today?"), cancellationToken);
                }
            }
        }
        private static async Task FillOutUserProfileAsync(ConversationFlow flow, UserProfile profile, ITurnContext turnContext)
        {
            string input = turnContext.Activity.Text?.Trim();
            string message;
            switch (flow.lastQuestion)
            {
                case ConversationFlow.Questions.None:
                    await turnContext.SendActivityAsync("Hey :) what's your name?");
                    flow.lastQuestion = ConversationFlow.Questions.Name;
                    return;
                case ConversationFlow.Questions.Name:
                    if(ValidateName(input, out string name, out message))
                    {
                        profile.name = name;
                        await turnContext.SendActivityAsync($"Hi {profile.name}.");
                        await turnContext.SendActivityAsync($"Now, could you please tell me what day your trip was?");
                        flow.lastQuestion = ConversationFlow.Questions.Date;
                        return;
                    }
                    else
                    {
                        await turnContext.SendActivityAsync($"Sorry, I didn't quite understand that...");
                        return;
                    }
                case ConversationFlow.Questions.Date:
                    if (true)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                //create an enum in conversationflow
            }
        }
        private static bool ValidateName(string input, out string name, out string message)
        {
            name = null;
            message = null;
            if (string.IsNullOrWhiteSpace(input))
            {
                message = "Please enter a name that contains at least one character";
            }
            else
            {
                name = input.Trim();
            }
            return message is null;
        }
    }
}
