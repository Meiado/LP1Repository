using Microsoft.AspNetCore.Mvc;

namespace IntroAPI2.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        public CustomControllerBase()
        {
            //Messages = new();
            Messages = new List<Message>();
        }

        public enum TypeMessage
        {
            Success = 1,
            InvalidField = 2,
            Error = 3,
            Information = 4,
            Alert = 5
        }

        public struct Message
        {
            public string Text { get; set; }
            public TypeMessage Type { get; set; }

        }

        public List<Message> Messages { get; set; } //= new List<Message>();

        public void AddSuccessMessage(string text)
        {
            Messages.Add(new Message()
            {
                Text = text,
                Type = TypeMessage.Success
            });
        }

        public IActionResult CustomResponseCreated(bool success, int id)
        {
            return CustomResponse(System.Net.HttpStatusCode.Created, success, id);

        }
        public IActionResult CustomResponse(System.Net.HttpStatusCode statusCode,
            bool success, object data = null)

        {
            var response = new
            {
                success = success,
                messages = Messages,
                data = data
            };

            return StatusCode((int)statusCode, response);
        }
    }
}
