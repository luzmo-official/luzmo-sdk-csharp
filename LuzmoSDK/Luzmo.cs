﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace LuzmoSDK
{

  public class Luzmo
  {
    public string app = "https://app.luzmo.com";
    public string host = "https://api.luzmo.com";
    public string port = "443";
    public string apiVersion = "0.1.0";
    public string apiKey;
    public string apiToken;

    public Luzmo(string apiKey, string apiToken)
    {
      this.apiKey = apiKey;
      this.apiToken = apiToken;
    }

    public Luzmo(string apiKey, string apiToken, string host, string port = "443")
    {
      this.apiKey = apiKey;
      this.apiToken = apiToken;
      this.host = host;
      this.port = port;
    }

    public Luzmo(string host, string port, string apiVersion, string apiKey, string apiToken)
    {
      this.host = host;
      this.port = port;
      this.apiKey = apiKey;
      this.apiToken = apiToken;
      this.apiVersion = apiVersion;
    }

    public dynamic create(string resource, ExpandoObject properties)
    {
      try {
        return createAsync(resource, properties).Result;
      }
      catch (AggregateException ae)
      {
        throw ae.InnerException;
      }
    }

    public dynamic create(string resource, ExpandoObject properties, List<ExpandoObject> associations)
    {
      try {
        return createAsync(resource, properties, associations).Result;
      }
      catch (AggregateException ae)
      {
        throw ae.InnerException;
      }
    }

    public async Task<dynamic> createAsync(string resource, ExpandoObject properties)
    {
      List<ExpandoObject> associations = new List<ExpandoObject>();
      return await createAsync(resource, properties, associations);
    }

    public async Task<dynamic> createAsync(string resource, ExpandoObject properties, List<ExpandoObject> associations)
    {
      LuzmoQuery query = new LuzmoQuery();
      query.action = "create";
      query.properties = properties;
      query.associations = associations;
      return await _emit(resource, "POST", query);
    }

    public dynamic get(string resource, ExpandoObject filter)
    {
      try {
        return getAsync(resource, filter).Result;
      }
      catch (AggregateException ae)
      {
        throw ae.InnerException;
      }
    }

    public async Task<dynamic> getAsync(string resource, ExpandoObject filter)
    {
      LuzmoQuery query = new LuzmoQuery();
      query.action = "get";
      query.find = filter;

      return await _emit(resource, "SEARCH", query);
    }

    public dynamic delete(string resource, string id, ExpandoObject properties)
    {
      try {
        return deleteAsync(resource, id, properties).Result;
      }
      catch (AggregateException ae)
      {
        throw ae.InnerException;
      }
    }

    public async Task<dynamic> deleteAsync(string resource, string id, ExpandoObject properties)
    {
      LuzmoQuery query = new LuzmoQuery();
      query.action = "delete";
      query.id = id;
      query.properties = properties;

      return await _emit(resource, "DELETE", query);
    }

    public dynamic update(string resource, string id, ExpandoObject properties)
    {
      try {
        return updateAsync(resource, id, properties).Result;
      }
      catch (AggregateException ae)
      {
        throw ae.InnerException;
      }
    }

    public async Task<dynamic> updateAsync(string resource, string id, ExpandoObject properties)
    {
      LuzmoQuery query = new LuzmoQuery();
      query.action = "update";
      query.id = id;
      query.properties = properties;

      return await _emit(resource, "PATCH", query);
    }

    public dynamic associate(string resource, string id, string associationRole, string associationId, ExpandoObject properties)
    {
      try {
        return associateAsync(resource, id, associationRole, associationId, properties).Result;
      }
      catch (AggregateException ae)
      {
        throw ae.InnerException;
      }
    }

    public async Task<dynamic> associateAsync(string resource, string id, string associationRole, string associationId, ExpandoObject properties)
    {
      dynamic association = new ExpandoObject();
      association.role = associationRole;
      association.id = associationId;

      LuzmoQuery query = new LuzmoQuery();
      query.action = "associate";
      query.id = id;
      query.resource = association;
      query.properties = properties;

      return await _emit(resource, "LINK", query);
    }

    public dynamic dissociate(string resource, string id, string associationRole, string associationId)
    {
      try {
        return dissociateAsync(resource, id, associationRole, associationId).Result;
      }
      catch (AggregateException ae)
      {
        throw ae.InnerException;
      }
    }

    public async Task<dynamic> dissociateAsync(string resource, string id, string associationRole, string associationId)
    {
      dynamic association = new ExpandoObject();
      association.role = associationRole;
      association.id = associationId;

      LuzmoQuery query = new LuzmoQuery();
      query.action = "dissociate";
      query.id = id;
      query.resource = association;

      return await _emit(resource, "UNLINK", query);
    }

    public dynamic query(ExpandoObject filter)
    {
      try {
        return queryAsync(filter).Result;
      }
      catch (AggregateException ae)
      {
        throw ae.InnerException;
      }
    }

    public async Task<dynamic> queryAsync(ExpandoObject filter)
    {
      LuzmoQuery query = new LuzmoQuery();
      query.action = "get";
      query.find = filter;

      return await _emit("data", "SEARCH", query);
    }

    /* Embedding */

    public String iframe(string dashboard, dynamic authorization)
    {
      return this.app + "/s/" + dashboard + "?key=" + authorization.id + "&token=" + authorization.token;
    }

    /* Helpers */

    private async Task<dynamic> _emit(string resource, string action, LuzmoQuery query)
    {
      query.key = apiKey;
      query.token = apiToken;
      query.version = apiVersion;

      string payload = JsonConvert.SerializeObject(query);
      string url = this.host + ':' + this.port + '/' + query.version + '/' + resource;
      string result = null;

      try {
        WebClient request = new WebClient();
        request.Headers.Add("Content-Type", "application/json");
        result = await request.UploadStringTaskAsync(url, action, payload).ConfigureAwait(false);
        return JsonConvert.DeserializeObject(result);
      }
      catch (WebException e) {
        if (e.Response == null)
          result = "{error: 'An unexpected error occurred. Please try again later!'}";
        else
          result = await (new StreamReader(e.Response.GetResponseStream()).ReadToEndAsync());
        throw new LuzmoException(JsonConvert.DeserializeObject(result));
      }
    }
  }

  class LuzmoQuery
  {
    public string key;
    public string token;
    public string version;
    public string action;
    public string id;
    public ExpandoObject properties;          // Create, update or associate fields
    public List<ExpandoObject> associations;  // Initial associations
    public ExpandoObject resource;            // Single association role
    public ExpandoObject find;                // Query filters
  }

  public class LuzmoException : Exception
  {
    public int code;
    public dynamic details;

    public LuzmoException()
    {
    }

    public LuzmoException(string message) : base(message)
    {
    }

    public LuzmoException(string message, Exception inner) : base(message, inner)
    {
    }

    public LuzmoException(dynamic result)
    {
      if (result.code != null)
      this.code = result.code;
      this.details = result;
    }
  }

}