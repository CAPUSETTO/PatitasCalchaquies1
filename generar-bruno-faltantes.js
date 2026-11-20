// Crea las requests de Bruno que faltan (GET por id, búsquedas, endpoints especiales, login).
// No sobrescribe archivos existentes.
const fs = require('fs');
const path = require('path');

const carpetaColeccion = './bruno-collection';
const B = '{{baseurl}}';

// Recursos con GET /{id}
const conGetById = [
  'Adopciones', 'Alimentaciones', 'AlojamientosAnimales', 'Animales', 'AtencionesVeterinarias',
  'DevolucionesAdopciones', 'Especies', 'EstadosAnimales', 'Esterilizaciones', 'FotosAnimales',
  'IngresosAnimales', 'Medicamento', 'Personas', 'PlanesAlimentacion', 'Predios', 'Roles',
  'Sectores', 'SeguimientosAdopciones', 'Sexos', 'SolicitudesAdopciones', 'Tratamientos',
  'TratamientosMedicamentos', 'Usuarios', 'Vacuna', 'Vacunaciones', 'Veterinarios',
];

// carpeta -> requests extra [archivo, nombre, metodo, url, body?]
const extras = {
  Personas: [
    ['Get By Dni.yml', 'Obtener Persona por DNI', 'GET', `${B}/api/Personas/dni/12345678`],
    ['Buscar por Nombre.yml', 'Buscar Personas por Nombre', 'GET', `${B}/api/Personas/buscar?nombre=Juan`],
  ],
  Medicamento: [['Buscar por Nombre.yml', 'Buscar Medicamentos por Nombre', 'GET', `${B}/api/Medicamento/buscar?nombre=Ivermectina`]],
  Vacuna: [['Buscar por Nombre.yml', 'Buscar Vacunas por Nombre', 'GET', `${B}/api/Vacuna/buscar?nombre=Rabia`]],
  Raza: [['Get By Especie.yml', 'Obtener Razas por Especie', 'GET', `${B}/api/Raza/especie/1`]],
  UsuariosRoles: [
    ['Get By Ids.yml', 'Obtener Usuario-Rol', 'GET', `${B}/api/UsuariosRoles/1/1`],
    ['Get By Usuario.yml', 'Obtener Roles de un Usuario', 'GET', `${B}/api/UsuariosRoles/usuario/1`],
  ],
  Usuarios: [
    ['Get Operadores.yml', 'Obtener Operadores', 'GET', `${B}/api/Usuarios/operadores`],
    ['Get Operador By Id.yml', 'Obtener Operador por ID', 'GET', `${B}/api/Usuarios/operadores/1`],
    ['Create Operador.yml', 'Crear Operador', 'POST', `${B}/api/Usuarios/operadores`, '{\n\n}'],
    ['Update Operador.yml', 'Actualizar Operador', 'PUT', `${B}/api/Usuarios/operadores/1`, '{\n\n}'],
    ['Cambiar Estado Operador.yml', 'Cambiar Estado de Operador', 'PATCH', `${B}/api/Usuarios/operadores/1/estado`, '{\n  "activo": true\n}'],
    ['Delete Operador.yml', 'Eliminar Operador', 'DELETE', `${B}/api/Usuarios/operadores/1`],
  ],
  Auth: [['Login.yml', 'Login', 'POST', `${B}/api/Auth/login`, '{\n  "usuario": "",\n  "password": ""\n}']],
};

const contenido = (nombre, metodo, url, seq, body) => `info:
  name: ${nombre}
  type: http
  seq: ${seq}

http:
  method: ${metodo}
  url: "${url}"${body ? `
  body:
    type: json
    data: |-
${body.split('\n').map((l) => '      ' + l).join('\n')}` : ''}
  auth: inherit

settings:
  encodeUrl: true
  timeout: 0
  followRedirects: true
  maxRedirects: 5
  forwardAuthorizationHeader: false
`;

const crear = (carpeta, archivo, nombre, metodo, url, body) => {
  const dir = path.join(carpetaColeccion, carpeta);
  fs.mkdirSync(dir, { recursive: true });
  const folderYml = path.join(dir, 'folder.yml');
  if (!fs.existsSync(folderYml)) {
    const n = fs.readdirSync(carpetaColeccion).length + 1;
    fs.writeFileSync(folderYml, `info:\n  name: ${carpeta}\n  type: folder\n  seq: ${n}\n\nrequest:\n  auth: inherit\n`);
  }
  const ruta = path.join(dir, archivo);
  if (fs.existsSync(ruta)) return console.log(`= ya existe: ${carpeta}/${archivo}`);
  const seq = fs.readdirSync(dir).filter((f) => f !== 'folder.yml').length + 1;
  fs.writeFileSync(ruta, contenido(nombre, metodo, url, seq, body), 'utf8');
  console.log(`+ ${carpeta}/${archivo}`);
};

conGetById.forEach((r) => crear(r, 'Get By Id.yml', `Obtener ${r} por ID`, 'GET', `${B}/api/${r}/1`));
Object.entries(extras).forEach(([c, reqs]) => reqs.forEach((r) => crear(c, ...r)));
