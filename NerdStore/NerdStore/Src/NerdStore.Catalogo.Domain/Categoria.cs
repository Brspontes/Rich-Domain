using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validar();
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        public string Nome { get; private set; }
        public int Codigo { get; private set; }

        //EF Relation
        public ICollection<Produto> Produtos { get; set; }

        protected Categoria() { }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo nome da categoria não pode estar vazio");
            Validacoes.ValidarSeIgual(Codigo, 0, "O campo codigo não pode ser 0");
        }
    }
}
