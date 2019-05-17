using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebPrison.Models
{
    public class NewDetension
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage ="Поле обязательно к заполнению")]
        [Display(Name ="Имя")]
        public string name { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Отчество")]
        public string surname { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Фамилия")]
        public string lastname { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Дата рождения")]
        public DateTime bday { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Место рождения")]
        public string b_place { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Место проживания")]
        public string live_place { get; set; }


        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Телефон")]
        public string phome { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Место работы(не обязательно)")]
        public string workplace { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Рабочий телефон(не обязательно)")]
        public  string workphone { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Место учебы(не обязательно)")]
        public string school { get; set; }

        [RegularExpression("([0-9]+)",ErrorMessage="Недопустимое значение")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Серия паспорта")]
        public string pass_ser { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Недопустимое значение")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Номер паспорта")]
        public string pass_num { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Дата задержания")]
        public DateTime det_date { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Причина задержания(не обязательно)")]
        public string det_reason { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания задержания(не обязательно)")]
        public DateTime det_end { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Часть/Статья(не обязательно)")]
        public string part { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Дата составления")]
        public DateTime date_sost { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Имя сотрудника")]
        public string officer_name { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Фамилия сотрудника")]
        public string officer_famil { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Должность(не обязательно)")]
        public string officer_post { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Звание(не обязательно)")]
        public string officer_rank { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Display(Name = "Место доставления")]
        public string compel_place { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Адрес доставления(не обязательно)")]
        public string compel_adress { get; set; }


    }
}