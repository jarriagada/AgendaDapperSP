﻿using AgendaDapperSP.Models;

namespace AgendaDapperSP.Repositorio
{
    public interface IRepositorio
    {
        Cliente GetCliente(int id);
        List<Cliente> GetClientes();
        Cliente AgregarCliente(Cliente cliente);
        /* Editar Cliente */
        Cliente ActualizarCliente(Cliente cliente);
        void BorrarCliente(int id);

    }
}
