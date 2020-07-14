using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using LaboratorioAzureCosmos.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace LaboratorioAzureCosmos.Controllers
{
    public class PersonaController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;

        private readonly IConfiguration _configuration;
        public PersonaController(ICosmosDbService cosmosDbService, IConfiguration configuration)
        {
            _cosmosDbService = cosmosDbService;
            _configuration = configuration;
        }



        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetItemsAsync("SELECT * FROM c"));
        }

        [ActionName("Create")]
        public IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Persona persona)
        {
            

            TempData[("Identificacion")] = persona.Identificacion;
            TempData[("Nombre")] = persona.Nombre;
            TempData[("PrimerApellido")] = persona.PrimerApellido;
            TempData[("SegundoApellido")] = persona.SegundoApellido;
            
            return RedirectToAction("CreateFoto");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePersona([Bind("id,Identificacion,Nombre,PrimerApellido,SegundoApellido,Foto")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                persona.id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddItemAsync(persona);
                return RedirectToAction("Index");
            }

            return View(persona);
        }
  
        public IActionResult CreateFoto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFoto(IFormFile files)
        {
            Persona persona = new Persona();
            persona.Identificacion = (int) TempData.Peek("Identificacion");
            persona.Nombre = (string) TempData.Peek("Nombre");
            persona.PrimerApellido = (string)TempData.Peek("PrimerApellido");
            persona.SegundoApellido = (string)TempData.Peek("SegundoApellido");

            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");

            byte[] dataFiles;

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("contenedordearchivos");
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            string systemFileName = (int)TempData.Peek("Identificacion") + files.FileName;
            await cloudBlobContainer.SetPermissionsAsync(permissions);
            await using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                dataFiles = target.ToArray();
            }

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
            await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

            string url = cloudBlockBlob.SnapshotQualifiedStorageUri.ToString();

            persona.Foto = url;

            return await CreatePersona(persona);
        }
    }
}