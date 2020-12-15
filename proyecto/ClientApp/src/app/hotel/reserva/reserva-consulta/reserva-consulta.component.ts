import { Component, OnInit } from '@angular/core';
import { ReservaService } from 'src/app/services/reserva.service';
import { Reserva } from '../../models/reserva';

@Component({
  selector: 'app-reserva-consulta',
  templateUrl: './reserva-consulta.component.html',
  styleUrls: ['./reserva-consulta.component.css']
})
export class ReservaConsultaComponent implements OnInit {

  reservas: Reserva[];
  constructor(private reservaService: ReservaService) { }
  
  ngOnInit(): void {
    this.reservas = [];
    this.actualizarListaSignal();
  }

  private actualizarListaSignal(){
    this.reservaService.signalRecived.subscribe((reserva: Reserva) => {
      this.reservas.push(reserva);
    });
  }

}
