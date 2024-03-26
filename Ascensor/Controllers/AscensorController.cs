using Microsoft.AspNetCore.Mvc;
using Ascensor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Ascensor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AscensorController : ControllerBase
    {
  
        private static Models.Ascensor ascensor = new Models.Ascensor();

        
        [HttpPost("inside")]
        public async Task<IActionResult> Move([FromBody] int targetFloor){
            
            if(targetFloor < 1){
                return BadRequest("En este edificio no hay sótano");
            }
            // Verifica si el ascensor ya está en el piso objetivo
            else if (ascensor.CurrentFloor == targetFloor)
            {
                return Ok("Ascensor está en el piso " + targetFloor);
            }
            else
            {
                
                List<int> pendingFloors = new List<int>();

                // Determina si el ascensor subirá o bajará para llegar al piso objetivo
                if (targetFloor > ascensor.CurrentFloor)
                {
                    // Agrega pisos pendientes para subir al objetivo
                    for (int i = ascensor.CurrentFloor + 1; i <= targetFloor; i++)
                    {
                        if (!pendingFloors.Contains(i))
                        {
                            pendingFloors.Add(i);
                        }
                    }
                }
                else
                {
                    // Agrega pisos pendientes para bajar al objetivo
                    for (int i = ascensor.CurrentFloor - 1; i >= targetFloor; i--)
                    {
                        if (!pendingFloors.Contains(i))
                        {
                            pendingFloors.Add(i);
                        }
                    }
                }

                // Agrega el piso objetivo a la lista total de pisos
                for (int i = 1; i <= targetFloor; i++)
                {
                    if (!ascensor.TotalFloors.Contains(i))
                    {
                        ascensor.TotalFloors.Add(i);
                    }
                }

                // Calcula la cantidad de pisos a mover
                int floorsToMove = Math.Abs(ascensor.CurrentFloor - targetFloor);

                
                await MoveElevatorAsync(floorsToMove);

                ascensor.CurrentFloor = targetFloor;

                return Ok(new { message = $"Ascensor moviéndose al piso {targetFloor}, pisos pendientes {string.Join(",", pendingFloors)}" });
            }
        }

        // Método para simular el movimiento del ascensor
        private async Task MoveElevatorAsync(int floorsToMove)
        {
            for (int i = 0; i < floorsToMove; i++)
            {
                // Espera 1 segundo antes de moverse al siguiente piso
                await Task.Delay(1000);
            }
        }

        // Método para llamar al ascensor desde un piso específico
        [HttpPost("outside")]
        public IActionResult Call([FromBody] int floor)
        {
            return Move(floor).Result;
        }

        // Método para obtener el estado actual del ascensor
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(ascensor);
        }
    }
}