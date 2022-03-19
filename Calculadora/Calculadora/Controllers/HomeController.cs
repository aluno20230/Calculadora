using Calculadora.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace Calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        [HttpGet]  // esta anotação é facultativa
        public IActionResult Index()
        {

            // inicializar a calculadora
            ViewBag.Visor = "0";


            return View();
        }



        [HttpPost]  // mas, esta anotação já é obrigatória, para o método 'escutar' o HTTP POST
        public IActionResult Index(
           string botao,
           string visor,
           string operador,
           string operando,
           string limpaVisor)
        {


            switch (botao)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    // atribuir ao VISOR o algarismo selecionado
                    if (limpaVisor != "sim" && visor != "0") visor = visor + botao;
                    else { visor = botao; }
                    // indica q o visor já não precisa de ser limpo
                    limpaVisor = "nao";
                    break;
                case ",":
                    // transforma o num inteiro em real
                    if (limpaVisor == "sim")
                    {
                        visor = "0,";
                        limpaVisor = "nao";
                    }
                    else
                    {
                        if (!visor.Contains(',')) visor += ",";
                    }
                    break;
                case "+/-":
                    //  inverte o valor do visor
                    if (visor.StartsWith('-')) visor = visor.Substring(1);
                    else visor = "-" + visor;
                    // poderíamos executar esta mesma operação de forma algébrica
                    break;
                case "+":
                case "-":
                case "x":
                case ":":
                    // processar as tarefas associadas aos operadores
                    if (!string.IsNullOrEmpty(operando))
                    {
                        // 2ª, 3ª, etc. escolha de operador
                        // agora temos mesmo de fazer as contas
                        double primeiroOperando = Convert.ToDouble(operando);
                        double segundoOperando = Convert.ToDouble(visor);
                        switch (operador)
                        {
                            case "+":
                                visor = Convert.ToString(primeiroOperando + segundoOperando);
                                break;
                            case "-":
                                visor = (primeiroOperando - segundoOperando).ToString();
                                break;
                            case "x":
                                visor = primeiroOperando * segundoOperando + "";
                                break;
                            case ":":
                                visor = primeiroOperando / segundoOperando + "";
                                break;                         
                        }
                    }              
                    // guardar os dados para a px. iteração
                    operador = botao;
                    operando = visor;
                    limpaVisor = "sim";

                    break;
                case "C":
                    visor = "";
                    operador = "";
                    operando = "";
                    break;
                case "=":
                    double primeiroOperandoteste = Convert.ToDouble(operando);
                    double segundoOperandoteste = Convert.ToDouble(visor);
                    if (operador.Contains("+")) { visor = (primeiroOperandoteste + segundoOperandoteste).ToString(); }
                    else if (operador.Contains("-")) { visor = (primeiroOperandoteste - segundoOperandoteste).ToString(); }
                    else if (operador.Contains("x")) { visor = (primeiroOperandoteste * segundoOperandoteste).ToString(); }
                    else { visor = (primeiroOperandoteste / segundoOperandoteste).ToString(); }
                    operador = "";
                    operando = "";
                    break;

            }

            // preparar dados a serem enviados para a View
            ViewBag.Visor = visor;
            ViewBag.Operando = operando;
            ViewBag.Operador = operador;
            ViewBag.LimpaVisor = limpaVisor;

            return View();
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}