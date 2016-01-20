using System;
using Commons.Utils;
using Commons.Utils.Commands;
using Facebook.Unity;
using RSG;

namespace Commons.SN.Facebook.Commands
{
    public class PostToFBCommand : IAsyncCommand
    {
        UnityEventProvider unityEvents;
        IPostData postData;

        public PostToFBCommand(UnityEventProvider _eventProvider)
        {
            unityEvents = _eventProvider;
        }

        public PostToFBCommand SetData(IPostData _data)
        {
            postData = _data;
            return this;
        }

        public IPromise<IAsyncCommand> Run()
        {
            var promise = new Promise<IAsyncCommand>();

            unityEvents.onGui.AddOnce(() =>
            {
                FB.FeedShare(
                    link: new Uri(postData.Link),
                    linkName: postData.LinkName,
                    linkCaption: postData.LinkCaption,
                    linkDescription: postData.LinkDescription,
                    picture: new Uri(postData.Picture),

                    callback: delegate (IShareResult _result)
                    {
                        if (_result.Error != null || _result.Cancelled)
                            promise.Reject(new Exception("Post failed!" + _result.Error));
                        else
                            promise.Resolve(this);
                    }
                );
            });

            return promise;
        }
    }
}