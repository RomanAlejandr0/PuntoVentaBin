﻿using System.ComponentModel.DataAnnotations;

namespace PuntoVentaBin.Shared.Identidades.Adm_PerfilTareas
{
    public class RolPermiso
    {
        [Key]
        public int RolID { get; set; }
        public Rol Rol { get; set; }

        public int PermisoID { get; set; }
        public Permiso Permiso { get; set; }

    }
}
