﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ContactsManager.Core.Enums;

using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Core.DTO
{
 public class RegisterDTO
 {
  [Required(ErrorMessage = "Name can't be blank")]
  public string PersonName { get; set; }


  [Required(ErrorMessage = "Email can't be blank")]
  [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
  [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already is use")]
  public string Email { get; set; }


  [Required(ErrorMessage = "Phone can't be blank")]
  [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain numbers only")]
  [DataType(DataType.PhoneNumber)]
  public string Phone { get; set; }


  [Required(ErrorMessage = "Password can't be blank")]
  [DataType(DataType.Password)]
  public string Password { get; set; }


  [Required(ErrorMessage = "Confirm Password can't be blank")]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "Passwords do not match")]
  public string ConfirmPassword { get; set; }

  public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
 }
}
