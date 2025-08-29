namespace inmobiliaria_benenatti.Models;

public class Inquilinos
{
    public int id { get; set; }

    public string dni { get; set; }="";

    public string nombre { get; set; }="";

    public string telefono { get; set; }="";

    public string? email { get; set; }

    public string direccion { get; set; }="";

}