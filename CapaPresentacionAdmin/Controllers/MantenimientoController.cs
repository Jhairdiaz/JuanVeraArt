using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

using CapaEntidad;
using CapaLogica;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;

namespace CapaPresentacionAdmin.Controllers
{

    [Authorize]
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult Portafolio()
        {
            return View();
        }



        public ActionResult Categoria()
        {
            return View();
        }


        public ActionResult Producto()
        {
            return View();
        }



        //********** METODOS PARA LA CATEGORIA **********//
        #region CATEGORIA

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();

            oLista = new LogicaCategoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                resultado = new LogicaCategoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new LogicaCategoria().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new LogicaCategoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //********** METODOS PARA EL PRODUCTO **********//
        #region PRODUCTO

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();

            oLista = new LogicaProducto().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {            
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);


            var NumStyles = NumberStyles.AllowDecimalPoint;
            var Cultureinfo = new CultureInfo("es-CO");

            decimal precio;

            if (decimal.TryParse(oProducto.PrecioTexto, NumStyles, Cultureinfo, out precio))
            {

          
                oProducto.Precio = precio;

                if (precio < 100)
                {
                    return Json(new { operacionExitosa = false, mensaje = "- El formato del precio debe ser sin decimales" }, JsonRequestBehavior.AllowGet);
                }
                //if (oProducto.PrecioTexto != "0")
                //{
                //    oProducto.PrecioTexto = oProducto.Precio;                                        
                //}
                //else
                //{
                //    return Json(new { operacionExitosa = false, mensaje = "- El precio no puede ser 0" }, JsonRequestBehavior.AllowGet);
                //}

            }
            else
            {                                
                return Json(new { operacionExitosa = false, mensaje = "- El formato del precio debe ser sin decimales" },JsonRequestBehavior.AllowGet);
            }

            //Registramos el producto en la DB
            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new LogicaProducto().Registrar(oProducto, out mensaje);

                if (idProductoGenerado !=0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }

            }
            else
            {
                operacion_exitosa = new LogicaProducto().Editar(oProducto, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    //Variables para guardar la imagen
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                      
                    }

                    if (guardar_imagen_exito)
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nombre_imagen;
                        bool rsta = new LogicaProducto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo un error con la imagen";
                    }
                }
            }            
            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            
        }


        //Metodo para subir la imagen del producto
        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oproducto = new LogicaProducto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = LogicaRecursos.ConvertirBase64(Path.Combine(oproducto.RutaImagen, oproducto.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oproducto.NombreImagen)

            },
            JsonRequestBehavior.AllowGet
            ); 
            
        }

        //Metodo para eliminar un producto
        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new LogicaProducto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //********** METODOS PARA EL PORTAFOLIO **********//
        #region PORTAFOLIO

        [HttpGet]
        public JsonResult ListarPortafolio()
        {
            List<Portafolio> oLista = new List<Portafolio>();

            oLista = new LogicaPortafolio().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPortafolio(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Portafolio oPortafolio = new Portafolio();
            oPortafolio = JsonConvert.DeserializeObject<Portafolio>(objeto);


            
            //Registramos el producto en la DB
            if (oPortafolio.IdPortafolio == 0)
            {
                int idProductoGenerado = new LogicaPortafolio().Registrar(oPortafolio, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oPortafolio.IdPortafolio = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }

            }
            else
            {
                operacion_exitosa = new LogicaPortafolio().Editar(oPortafolio, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    //Variables para guardar la imagen
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotosPortafolio"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oPortafolio.IdPortafolio.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;

                    }

                    if (guardar_imagen_exito)
                    {
                        oPortafolio.RutaImagen = ruta_guardar;
                        oPortafolio.NombreImagen = nombre_imagen;
                        bool rsta = new LogicaPortafolio().GuardarDatosImagen(oPortafolio, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo pero hubo un error con la imagen";
                    }
                }
            }
            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oPortafolio.IdPortafolio, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        //Metodo para subir la imagen del producto
        [HttpPost]
        public JsonResult ImagenPortafolio(int id)
        {
            bool conversion;
            Portafolio oPortafolio = new LogicaPortafolio().Listar().Where(p => p.IdPortafolio == id).FirstOrDefault();

            string textoBase64 = LogicaRecursos.ConvertirBase64(Path.Combine(oPortafolio.RutaImagen, oPortafolio.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oPortafolio.NombreImagen)

            },
            JsonRequestBehavior.AllowGet
            );

        }

        //Metodo para eliminar un producto
        [HttpPost]
        public JsonResult EliminarPortafolio(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new LogicaPortafolio().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion  
    }
}