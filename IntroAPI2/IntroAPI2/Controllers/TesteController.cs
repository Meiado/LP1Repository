using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IntroAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {

        //regras básicas da API
        // - Usar mecanismo de LOG
        // - Usar verbo HTTP nas requisições
        // - Retornar sempre um código HTTP
        // - Padronizar os retornos da API

        [HttpGet]
        [Route("[action]")]
        public IActionResult TesteGet()
        {
            return Ok();
        }

        [HttpGet]
        [Route("[action]/{id}/{nome}")]
        public IActionResult TesteGet2(int id,  string nome)
        {
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ExemploPost(JsonElement dados)
        {
            int id = dados.GetProperty("id").GetInt32();
            string nome = dados.GetProperty("nome").GetString();
            return Ok(dados);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ExemploPost2(ViewModel.ExemploPost2ViewModel dados)
        {

            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ExemploPost3()
        {
            if(!Request.Form.Files.Any())
            {
                using (var ms = new MemoryStream())
                {
                    Request.Form.Files[0].CopyTo(ms);
                    string nome = Request.Form.Files[0].FileName;
                    string tipo = Request.Form.Files[0].ContentType;
                    var arq = ms.ToArray();
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult TesteGet3()
        {
            byte[] arq = System.IO.File.ReadAllBytes(@"c:\caminhodoarquivo");
            return File(arq, "text/plain", "arq.txt");
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult TestePut(int id, JsonElement dados)
        {
            string nome = dados.GetProperty("name").GetString();
            return Ok();
        }
    }
}
