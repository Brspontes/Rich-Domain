using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain
{
    public class Dimensoes
    {
        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {
            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;

            Validacoes.ValidarSeMenorIgualMinimo(altura, 1, "O campo altura deve ser maior que ");
            Validacoes.ValidarSeMenorIgualMinimo(largura, 1, "O campo largura deve ser maior que ");
            Validacoes.ValidarSeMenorIgualMinimo(profundidade, 1, "O campo profundidade deve ser maior que ");
        }

        public decimal Altura { get; private set; }
        public decimal Largura { get; private set; }
        public decimal Profundidade { get; private set; }

        public string DescricaoFormatada() =>
            $"LxAxP: {Largura} x {Altura} x {Profundidade}";

        public override string ToString()
        {
            return DescricaoFormatada(); 
        }

    }
}
