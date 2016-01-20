using System;

namespace Commons.SN
{
    public class PostData : IPostData
    {
        // public PostData(
        //     string link,
        //     string linkName,
        //     string linkCaption,
        //     string linkDescription,
        //     string picture)
        // {
        //     Link = new Uri(link),
                
        // }
        
        public string Link
        {
            get; set;
        }

        public string LinkCaption
        {
            get; set;
        }

        public string LinkDescription
        {
            get; set;
        }

        public string LinkName
        {
            get; set;
        }

        public string Picture
        {
            get; set;
        }
    }
}