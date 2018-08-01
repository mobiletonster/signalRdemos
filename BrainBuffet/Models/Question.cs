using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBuffet.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int ShowNumber { get; set; }
        public DateTime AirDate { get; set; }
        public string Round { get; set; }
        public string Category { get; set; }
        public string Value { get; set; }
        public string QuestionType { get; set; }
        public bool Media { get; set; }
        public string MediaType { get
            {
                if (Media)
                {
                    var ext = MediaUrl.Split('.').Last();
                    switch (ext)
                    {
                        case "jpg":
                        case "jpeg":
                        case "gif":
                        case "png":
                        case "bmp":
                            return "image";
                        case "wav":
                        case "mp3":
                            return "audio";
                        case "wmv":
                        case "avi":
                        case "mp4":
                        case "m4v":
                            return "video";
                        default:
                            return "text";
                    }
                }
                return "text";
            } }
        public string MediaUrl { get; set; }
        public string ImageUrl { get; set; }
        [JsonProperty("Question")]
        public string  question { get; set; }
        [JsonProperty("Answer")]
        public string Answer { get; set; }
        public string QuestionText { get { return question; } set { question = value; } }
        public string AnswerText { get { return Answer; }  set { Answer = value; } }
    }
}
