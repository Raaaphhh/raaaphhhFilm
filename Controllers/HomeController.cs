using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using raaaphhhFilm.Models;

namespace raaaphhhFilm.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    private readonly Dictionary<string, (string Url, bool IsPrivate, string CodeSecret)> videos = new()
    {
        {"SBB VOL 1", ("https://www.youtube.com/embed/vNUZywj2T54?si=qRbN9l7KfkMSoSze", false, "")},
        {"FAM", ("https://www.youtube.com/embed/B_Ck2-ad0D0?si=BF25tKSnNqobC63z", false, "")},
        {"DAMS16", ("https://www.youtube.com/embed/GkXt5Lvq2jc?si=Jy4twxwnFD9Pu8XE", false, "")},
        {"DAMS16 x RAPH", ("https://www.youtube.com/embed/6ye9XPKGDaE?si=hrzF6_LtJlPnluHS", false, "")},
        {"SHAME", ("https://www.youtube.com/embed/0mtOnGEon5g?si=gGcUMEQ0-VqsGjjH", false, "")},
        {"ST GILLES V2", ("https://www.youtube.com/embed/tux4geFIM_U?si=57-TY_SkrrHtB_uP", false, "")},
        {"Its My Time", ("https://www.youtube.com/embed/Ij5bwxIJ0AY?si=euRxLQrfQsrct1eY", false, "")},
        {"Dreux Vibes", ("https://www.youtube.com/embed/lf6F53YZFPo?si=gVvWGLXRUH0B9JL2", false, "")},
        {"Parissy", ("https://www.youtube.com/embed/756z90Dfp8I?si=SwnXYxv9-eXopU3H", false, "")},
        {"KOLD", ("https://www.youtube.com/embed/wV1lRdTTwWg?si=__iJ6ikY5uwtVp3f", false, "")},
        {"Alone", ("https://www.youtube.com/embed/P8hpnjseRDQ?si=tgU0lC54Q9W4ngTh", true, "1605")},
        {"NYW", ("https://www.youtube.com/embed/RzjV06Qanck?si=EsH_asXzvl7tutM3", true, "1605")},
    };
    
    private const string GlobalCode = "1605";


    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.CodeValid = false; 
        ViewBag.Videos = videos;
        return View();
    }

    [HttpPost]
    public IActionResult Index(string Code)
    {
        bool codeValid = Code == GlobalCode;

        ViewBag.CodeValid = codeValid;
        ViewBag.Videos = videos;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}