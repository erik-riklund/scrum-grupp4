﻿using App.Entities;
using App.Interfaces;

namespace App.Services
{
  public class SessionManager(HttpContext context) : ISessionManager
  {
    public async Task<User?> GetUserAsync()
    {
      if (context.User.FindFirst("ID")?.Value is string ID)
      {
        return await Query.FetchOneById<User>(ID);
      }

      return null;
    }

    public bool IsAuthenticated() => context.User.Identity?.IsAuthenticated == true;

    public async Task<bool> HasRoleAsync(string roleName) =>
      (await GetUserAsync())?.Roles.Where(role => role.Name.Equals(roleName)).Any() == true;
  }
}
