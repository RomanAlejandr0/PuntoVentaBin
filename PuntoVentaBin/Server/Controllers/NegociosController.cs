﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuntoVentaBin.Shared;
using PuntoVentaBin.Shared.AccesoDatos;
using PuntoVentaBin.Shared.Identidades;


namespace PuntoVentaBin.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NegociosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public NegociosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetAll/{usuarioId}")]
        public async Task<Respuesta<List<Negocio>>> Get(long usuarioId)
        {
            var respuesta = new Respuesta<List<Negocio>>() { Estado = EstadosDeRespuesta.Correcto };

            try
            {
                respuesta.Datos = await context.UsuariosRolesNegocios
                       .Where(un => un.UsuarioId == usuarioId)
                       .Select(un => un.Negocio)
                       .AsNoTracking()
                       .ToListAsync();

            }
            catch (Exception e)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Mensaje = e.InnerException.ToString();
            }

            return respuesta;
        }


        [HttpPost]
        [Route("{action}")]
        public async Task<Respuesta<long>> GuardarNegocio([FromBody] Negocio negocio)
        {
            var respuesta = new Respuesta<long> { Estado = EstadosDeRespuesta.Correcto, Mensaje = "Guardado Correctamente" };
            var transaction = context.Database.BeginTransaction();

            try
            {
                negocio.FechaRegistro = DateTime.Now;

                //negocio.Usuarios.Last().RolId = 1;
                //negocio.Usuarios.Last().FechaRegistro = DateTime.Now;

                negocio.Clientes.Last().Nombre = "Cliente Generico";
                negocio.Clientes.Last().Email = "";

                context.Negocios.Add(negocio);

                await context.SaveChangesAsync(true);

                respuesta.Datos = negocio.Id;

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Ocurrio un error al guardar la empresa";
            }
            return respuesta;
        }

    }
}