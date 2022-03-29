﻿using System.ComponentModel.DataAnnotations;

namespace Vets.Models {
    public class Donos {
        
        /// <summary>
        /// Representa os dados do Dono de um animal
        /// </summary>
        public Donos() {
            ListaAnimais = new HashSet<Animais>();    
        }

        /// <summary>
        /// PK para a tabela dos Donos
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do dono do aninal
        /// </summary>
        [Required(ErrorMessage = "O {0} é preenchimento obrigatório")]
        public string Nome { get; set; }

        /// <summary>
        /// NIF do dono do animal
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string NIF { get; set; }
        
        /// <summary>
        /// sexo do dono
        /// Ff - feminino / Mm - masculino
        /// </summary>

        public string Sexo { get; set; }

        /// <summary>
        /// lista dos animais de quem o Dono é dono
        /// </summary>
        public ICollection<Animais> ListaAnimais { get; set; }

    }
}
