using System;
using System.IO; 

class Program
{
    
    static int MAX_PACIENTES = 1000;
    static string[] arreglo_DNI = new string[MAX_PACIENTES];
    static string[] arreglo_Nombres = new string[MAX_PACIENTES];
    static string[] arreglo_Especialidad = new string[MAX_PACIENTES];
    static string[] arreglo_Sexo = new string[MAX_PACIENTES];
    static string[] arreglo_FechaHora = new string[MAX_PACIENTES];
    static int total_pacientes = 0;

    
    static string ruta_archivo = "pacientes.txt";

    static void Main(string[] args)
    {
       
        CargarDatos();

        int opcion = 0;

       
        while (opcion != 6)
        {
            Console.WriteLine("\n==============================================");
            Console.WriteLine("  MESA DE PARTES DIGITAL - CENTRO MÉDICO");
            Console.WriteLine("==============================================");
            Console.WriteLine("1. Ingresar nuevo expediente");
            Console.WriteLine("2. Buscar historia por DNI");
            Console.WriteLine("3. Ver lista de pacientes del día");
            Console.WriteLine("4. Editar historia clínica (Corregir error)");
            Console.WriteLine("5. Guardar datos en archivo local (.txt)");
            Console.WriteLine("6. Salir");
            Console.Write("Ingrese su opción: ");

            string entrada = Console.ReadLine();

           
            if (int.TryParse(entrada, out opcion))
            {
                if (opcion == 1) RegistrarPaciente();
                else if (opcion == 2) BuscarPaciente();
                else if (opcion == 3) MostrarPacientes();
                else if (opcion == 4) EditarPaciente();
                else if (opcion == 5) GuardarDatos(true);
                else if (opcion == 6)
                {
                    GuardarDatos(false); 
                    Console.WriteLine("\nCerrando sistema de recepción. ¡Hasta pronto!");
                }
                else Console.WriteLine("\nOpción no válida. Ingrese un número del 1 al 6.");
            }
            else
            {
                Console.WriteLine("\nError: Por favor, ingrese solo números.");
            }
        }
    }

    

    static void CargarDatos()
    {
       
        if (File.Exists(ruta_archivo))
        {
            string[] lineas = File.ReadAllLines(ruta_archivo);
            foreach (string linea in lineas)
            {
               
                string[] datos = linea.Split(',');

                
                if (datos.Length == 5)
                {
                    arreglo_FechaHora[total_pacientes] = datos[0];
                    arreglo_DNI[total_pacientes] = datos[1];
                    arreglo_Nombres[total_pacientes] = datos[2];
                    arreglo_Sexo[total_pacientes] = datos[3];
                    arreglo_Especialidad[total_pacientes] = datos[4];
                    total_pacientes++;
                }
            }
            Console.WriteLine("--> Historial clínico cargado exitosamente desde el disco local.");
        }
        else
        {
            Console.WriteLine("--> No se encontró historial previo. Se creará una nueva base de datos hoy.");
        }
    }

    static void GuardarDatos(bool mostrarAviso)
    {
       
        using (StreamWriter archivo = new StreamWriter(ruta_archivo))
        {
            for (int i = 0; i < total_pacientes; i++)
            {
               
                string linea = $"{arreglo_FechaHora[i]},{arreglo_DNI[i]},{arreglo_Nombres[i]},{arreglo_Sexo[i]},{arreglo_Especialidad[i]}";
                archivo.WriteLine(linea);
            }
        }

        if (mostrarAviso)
        {
            Console.WriteLine($"\n¡Éxito! Todos los datos han sido guardados de forma segura en '{ruta_archivo}'.");
        }
    }

    static void RegistrarPaciente()
    {
        if (total_pacientes < MAX_PACIENTES)
        {
            Console.WriteLine("\n--- REGISTRAR NUEVO PACIENTE ---");

          
            while (true)
            {
                Console.Write("Ingrese el DNI (solo 8 números): ");
                string dni_temp = Console.ReadLine();
                if (dni_temp.Length == 8 && int.TryParse(dni_temp, out _))
                {
                    arreglo_DNI[total_pacientes] = dni_temp;
                    break;
                }
                Console.WriteLine("Error: El DNI debe contener exactamente 8 números. Inténtelo de nuevo.");
            }

           
            while (true)
            {
                Console.Write("Ingrese Nombres y Apellidos (solo letras): ");
                string nombre_temp = Console.ReadLine();
                bool esValido = true;

                foreach (char letra in nombre_temp)
                {
                    if (!char.IsLetter(letra) && letra != ' ')
                        esValido = false;
                }

                if (esValido && nombre_temp.Length > 0)
                {
                    arreglo_Nombres[total_pacientes] = nombre_temp;
                    break;
                }
                Console.WriteLine("Error: El nombre solo puede contener letras. Inténtelo de nuevo.");
            }

            
            while (true)
            {
                Console.Write("Seleccione el sexo (1: Masculino | 2: Femenino): ");
                string opcion_sexo = Console.ReadLine();

                if (opcion_sexo == "1")
                {
                    arreglo_Sexo[total_pacientes] = "Masculino";
                    break;
                }
                else if (opcion_sexo == "2")
                {
                    arreglo_Sexo[total_pacientes] = "Femenino";
                    break;
                }
                Console.WriteLine("Error: Ingrese solo 1 o 2.");
            }

            Console.Write("Ingrese la Especialidad requerida (ej. Pediatría): ");
            arreglo_Especialidad[total_pacientes] = Console.ReadLine();

            
            arreglo_FechaHora[total_pacientes] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            total_pacientes++;
            Console.WriteLine("¡Paciente registrado exitosamente!");
        }
        else
        {
            Console.WriteLine("\nError: Memoria llena. No caben más pacientes.");
        }
    }

    static void BuscarPaciente()
    {
        Console.WriteLine("\n--- BUSCAR HISTORIA CLÍNICA ---");
        Console.Write("Ingrese el DNI exacto a buscar: ");
        string dni_buscado = Console.ReadLine();
        bool encontrado = false;

        for (int i = 0; i < total_pacientes; i++)
        {
            if (arreglo_DNI[i] == dni_buscado)
            {
                Console.WriteLine("\n--- HISTORIA CLÍNICA ENCONTRADA ---");
                Console.WriteLine("Fecha de Registro: " + arreglo_FechaHora[i]);
                Console.WriteLine("DNI: " + arreglo_DNI[i]);
                Console.WriteLine("Paciente: " + arreglo_Nombres[i]);
                Console.WriteLine("Sexo: " + arreglo_Sexo[i]);
                Console.WriteLine("Especialidad: " + arreglo_Especialidad[i]);
                encontrado = true;
                break;
            }
        }

        if (encontrado == false)
        {
            Console.WriteLine("\nEl DNI ingresado no cuenta con una historia registrada.");
        }
    }

    static void MostrarPacientes()
    {
        Console.WriteLine("\n--- LISTA DE PACIENTES REGISTRADOS ---");
        if (total_pacientes == 0)
        {
            Console.WriteLine("Aún no hay pacientes registrados en el sistema.");
        }
        else
        {
            for (int i = 0; i < total_pacientes; i++)
            {
                Console.WriteLine($"{i + 1}. [{arreglo_FechaHora[i]}] DNI: {arreglo_DNI[i]} | {arreglo_Nombres[i]} | {arreglo_Sexo[i]} | {arreglo_Especialidad[i]}");
            }
        }
    }

    static void EditarPaciente()
    {
        Console.WriteLine("\n--- EDITAR HISTORIA CLÍNICA ---");
        Console.Write("Ingrese el DNI del paciente que desea corregir: ");
        string dni_buscado = Console.ReadLine();
        bool encontrado = false;

        for (int i = 0; i < total_pacientes; i++)
        {
            if (arreglo_DNI[i] == dni_buscado)
            {
                encontrado = true;
                Console.WriteLine("\nPaciente encontrado: " + arreglo_Nombres[i]);
                Console.WriteLine("¿Qué dato se equivocó y desea corregir?");
                Console.WriteLine("1. Nombres y Apellidos");
                Console.WriteLine("2. Sexo");
                Console.WriteLine("3. Especialidad");
                Console.WriteLine("4. Cancelar y regresar");
                Console.Write("Ingrese su opción: ");

                string opcion_editar = Console.ReadLine();

                if (opcion_editar == "1")
                {
                    while (true)
                    {
                        Console.Write("Ingrese el nuevo Nombre correcto (solo letras): ");
                        string nombre_temp = Console.ReadLine();
                        bool esValido = true;

                        foreach (char letra in nombre_temp)
                        {
                            if (!char.IsLetter(letra) && letra != ' ')
                                esValido = false;
                        }

                        if (esValido && nombre_temp.Length > 0)
                        {
                            arreglo_Nombres[i] = nombre_temp;
                            Console.WriteLine("¡Nombre corregido exitosamente!");
                            break;
                        }
                        Console.WriteLine("Error: El nombre solo puede contener letras.");
                    }
                }
                else if (opcion_editar == "2")
                {
                    while (true)
                    {
                        Console.Write("Seleccione el sexo correcto (1: Masculino | 2: Femenino): ");
                        string op_sexo = Console.ReadLine();
                        if (op_sexo == "1") { arreglo_Sexo[i] = "Masculino"; Console.WriteLine("¡Sexo corregido!"); break; }
                        else if (op_sexo == "2") { arreglo_Sexo[i] = "Femenino"; Console.WriteLine("¡Sexo corregido!"); break; }
                        Console.WriteLine("Error: Ingrese solo 1 o 2.");
                    }
                }
                else if (opcion_editar == "3")
                {
                    Console.Write("Ingrese la nueva Especialidad correcta: ");
                    arreglo_Especialidad[i] = Console.ReadLine();
                    Console.WriteLine("¡Especialidad corregida exitosamente!");
                }
                else if (opcion_editar == "4")
                {
                    Console.WriteLine("Edición cancelada.");
                }
                else
                {
                    Console.WriteLine("Opción no válida.");
                }
                break;
            }
        }

        if (encontrado == false)
        {
            Console.WriteLine("\nEl DNI ingresado no está registrado.");
        }
    }
}