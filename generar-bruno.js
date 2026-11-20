const fs = require('fs');
const path = require('path');

// 1. Clases/entidades de la API (ruta = api/[controller])
const recursos = [
  { nombre: 'Adopciones', ruta: 'api/Adopciones' },
  { nombre: 'Alimentaciones', ruta: 'api/Alimentaciones' },
  { nombre: 'AlojamientosAnimales', ruta: 'api/AlojamientosAnimales' },
  { nombre: 'Animales', ruta: 'api/Animales' },
  { nombre: 'AtencionesVeterinarias', ruta: 'api/AtencionesVeterinarias' },
  { nombre: 'DevolucionesAdopciones', ruta: 'api/DevolucionesAdopciones' },
  { nombre: 'Especies', ruta: 'api/Especies' },
  { nombre: 'EstadosAnimales', ruta: 'api/EstadosAnimales' },
  { nombre: 'Esterilizaciones', ruta: 'api/Esterilizaciones' },
  { nombre: 'FotosAnimales', ruta: 'api/FotosAnimales' },
  { nombre: 'IngresosAnimales', ruta: 'api/IngresosAnimales' },
  { nombre: 'Medicamento', ruta: 'api/Medicamento' },
  { nombre: 'Personas', ruta: 'api/Personas' },
  { nombre: 'PlanesAlimentacion', ruta: 'api/PlanesAlimentacion' },
  { nombre: 'Predios', ruta: 'api/Predios' },
  { nombre: 'Raza', ruta: 'api/Raza' },
  { nombre: 'Roles', ruta: 'api/Roles' },
  { nombre: 'Sectores', ruta: 'api/Sectores' },
  { nombre: 'SeguimientosAdopciones', ruta: 'api/SeguimientosAdopciones' },
  { nombre: 'Sexos', ruta: 'api/Sexos' },
  { nombre: 'SolicitudesAdopciones', ruta: 'api/SolicitudesAdopciones' },
  { nombre: 'Tratamientos', ruta: 'api/Tratamientos' },
  { nombre: 'TratamientosMedicamentos', ruta: 'api/TratamientosMedicamentos' },
  { nombre: 'Usuarios', ruta: 'api/Usuarios' },
  { nombre: 'UsuariosRoles', ruta: 'api/UsuariosRoles' },
  { nombre: 'Vacuna', ruta: 'api/Vacuna' },
  { nombre: 'Vacunaciones', ruta: 'api/Vacunaciones' },
  { nombre: 'Veterinarios', ruta: 'api/Veterinarios' }
];

// Carpeta base donde está tu colección de Bruno (ajústala si es necesario)
const carpetaColeccion = './bruno-collection';

// Variable de entorno que usas en Bruno para la URL base (ej. http://localhost:5227)
const baseUrl = '{{baseurl}}';

// Plantilla para crear cada request en formato YAML (Bruno 4.x / OpenCollection)
const crearContenidoYml = (nombreReq, metodo, url, seq) => `info:
  name: ${nombreReq}
  type: http
  seq: ${seq}

http:
  method: ${metodo}
  url: "${url}"${['POST', 'PUT'].includes(metodo) ? `
  body:
    type: json
    data: |-
      {

      }` : ''}
  auth: inherit

settings:
  encodeUrl: true
  timeout: 0
  followRedirects: true
  maxRedirects: 5
  forwardAuthorizationHeader: false
`;

// Generación de carpetas y archivos
recursos.forEach((recurso, i) => {
  const dirPath = path.join(carpetaColeccion, recurso.nombre);

  // Crea la carpeta de la clase si no existe
  fs.mkdirSync(dirPath, { recursive: true });

  fs.writeFileSync(
    path.join(dirPath, 'folder.yml'),
    `info:\n  name: ${recurso.nombre}\n  type: folder\n  seq: ${i + 1}\n\nrequest:\n  auth: inherit\n`,
    'utf8'
  );

  const endpoints = [
    { archivo: 'Get All.yml', nombre: `Obtener ${recurso.nombre}`, metodo: 'GET', url: `${baseUrl}/${recurso.ruta}`, seq: 1 },
    { archivo: 'Create.yml', nombre: `Crear ${recurso.nombre}`, metodo: 'POST', url: `${baseUrl}/${recurso.ruta}`, seq: 2 },
    { archivo: 'Update.yml', nombre: `Actualizar ${recurso.nombre}`, metodo: 'PUT', url: `${baseUrl}/${recurso.ruta}/1`, seq: 3 },
    { archivo: 'Delete.yml', nombre: `Eliminar ${recurso.nombre}`, metodo: 'DELETE', url: `${baseUrl}/${recurso.ruta}/1`, seq: 4 },
  ];

  endpoints.forEach((ep) => {
    const filePath = path.join(dirPath, ep.archivo);
    fs.writeFileSync(filePath, crearContenidoYml(ep.nombre, ep.metodo, ep.url, ep.seq), 'utf8');
  });

  console.log(`✅ Carpeta y endpoints creados para: ${recurso.nombre}`);
});
