using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NerdStore.Core.DomainObjects
{
    public class Validacoes
    {
        public static void ValidarSeIgual(object object1, object object2, string messagem)
        {
            if (!object1.Equals(object2))
                throw new DomainException(messagem);
        }

        public static void ValidarSeDiferente(object object1, object object2, string messagem)
        {
            if (object1.Equals(object2))
                throw new DomainException(messagem);
        }

        public static void ValidarCaracteres(string valor, int maximo, string mensagem)
        {
            var lenght = valor.Trim().Length;
            if (lenght > maximo)
                throw new DomainException(mensagem);
        }

        public static void ValidarCaracteres(string valor, int minimo, int maximo, string mensagem)
        {
            var lenght = valor.Trim().Length;
            if (lenght < minimo || lenght > maximo)
                throw new DomainException(mensagem);
        }

        public static void ValidarExpressao(string pattern, string valor, string mensagem)
        {
            var regex = new Regex(pattern);
            if (!regex.IsMatch(valor))
                throw new DomainException(mensagem);
        }

        public static void ValidarSeVazio(string valor, string mensagem)
        {
            if (valor is null || valor.Trim().Length == 0)
                throw new DomainException(mensagem);
        }

        public static void ValidarSeNulo(object object1, string mensagem)
        {
            if (object1 is null)
                throw new DomainException(mensagem);
        }

        public static void ValidarMinimoMaximo(double valor, double minimo, double maximo, string mensagem)
        {
            if (valor < minimo || valor > maximo)
                throw new DomainException(mensagem);
        }

        public static void ValidarMinimoMaximo(float valor, float minimo, float maximo, string mensagem)
        {
            if (valor < minimo || valor > maximo)
                throw new DomainException(mensagem);
        }
    }
}
