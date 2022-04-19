namespace Vets.Models {
    public class Veterinarios {
        /// <summary>
        /// modelo que interage com os dados
        /// </summary>
        public Veterinarios() {
            ListaConsultas = new HashSet<Consultas>();
        }
        /// <summary>
        /// ID do veterinário
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome do veterinário
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Nº da cedula profissional
        /// </summary>
        public string NumCedulaProf { get; set; }
        /// <summary>
        /// Nome do ficheiro que contem a fotografia do Veterinario
        /// </summary>
        public string Fotografia { get; set; }
        /// <summary>
        /// Lista das consultas
        /// </summary>
        public ICollection<Consultas> ListaConsultas { get; set; }

    }
}
