using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaLogica;

namespace CapaPresentacionTienda.Controllers
{
    public class HomeController : Controller
    {
        // GET: Tienda

        public ActionResult Home()
        {
            return View();
        }


        public ActionResult Portafolio()
        {
            return View();
        }


        public ActionResult Tienda()
        {
            return View();
        }


        public ActionResult Contacto()
        {
            return View();
        }


        public ActionResult DetalleProducto(int idproducto = 0)
        {

            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new LogicaProducto().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = LogicaRecursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);

            }

            return View(oProducto);
        }


        //Metodo que nos devuelve la lista de categorias
        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new LogicaCategoria().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

      



        [HttpPost]
        public JsonResult ListaProducto(int idcategoria)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;

            lista = new LogicaProducto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = LogicaRecursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
                p.oCategoria.IdCategoria == (idcategoria == 0 ? p.oCategoria.IdCategoria : idcategoria) &&
                p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }


        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            //Obtenemos el id del cliente
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool existe = new LogicaCarrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;

            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new LogicaCarrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            //Id del cliente
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new LogicaCarrito().CantidadEnCarrito(idcliente);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaPortafolio()
        {
            List<Portafolio> lista = new List<Portafolio>();
            bool conversion;

            lista = new LogicaPortafolio().Listar().Select(p => new Portafolio()
            {
                IdPortafolio = p.IdPortafolio,
                Descripcion = p.Descripcion,
                RutaImagen = p.RutaImagen,
                Base64 = LogicaRecursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p => p.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }


        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<Carrito> oLista = new List<Carrito>();
            bool conversion;

            oLista = new LogicaCarrito().ListarProducto(idcliente).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    IdProducto = oc.oProducto.IdProducto,
                    Descripcion = oc.oProducto.Descripcion,
                    oCategoria = oc.oProducto.oCategoria,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 = LogicaRecursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)

                },
                Cantidad = oc.Cantidad

            }).ToList();

            var jsonresult = Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
                
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            //Obtenemos el id del cliente
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            
            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new LogicaCarrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);
           
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new LogicaCarrito().EliminarCarrito(idcliente, idproducto);
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();

            oLista = new LogicaUbicacion().ObtenerDepartamento();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerCiudad(string IdDepartamento)
        {
            List<Ciudad> oLista = new List<Ciudad>();

            oLista = new LogicaUbicacion().ObtenerCiudad(IdDepartamento);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Carrito()
        {
            return View();

        }
    }

    

}