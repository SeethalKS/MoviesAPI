import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { icon, latLng, LeafletMouseEvent, marker, Marker, tileLayer } from 'leaflet';
import {LeafletModule} from '@bluehalo/ngx-leaflet';
import { Coordinate } from './Coordinate.model';

@Component({
  selector: 'app-map',
  imports: [LeafletModule],
  templateUrl: './map.component.html',
  styleUrl: './map.component.css'
})
export class MapComponent implements OnInit{
 

    @Input()
    initialCoordinates:Coordinate[] =[];

    @Input()
    readOnlyMode:boolean = false;
    
  @Output()
  coordinatedSelected = new EventEmitter<Coordinate>();


  ngOnInit(): void {
    this.layers = this.initialCoordinates.map(value =>
    {
      const myMarker= marker([value.latitude,value.longitude],this.markerOptions);
      if(value.text){
        myMarker.bindPopup(value.text,{autoClose:false,autoPan:false});
      }
      return myMarker;
    }
    );
  }


  markerOptions={
    icon:icon({
      iconSize:[25,41],
      iconAnchor:[13,41],
      iconUrl:'assets/marker-icon.png',
      iconRetinaUrl:'assets/marker-icon-2x.png',
      shadowUrl:'assets/marker-shadow.png'
    })
  }
  options={
    layers:[
      tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',{
        maxZoom:18,
        attribution:'...'
      })
    ],
    zoom:14,
    center:latLng(25.24231295550149, 51.44158809603081)
  }

  layers: Marker<any>[] = [];
  
  handleClick(event:LeafletMouseEvent){
    if(this.readOnlyMode){
      return;
    }
    const latitude = event.latlng.lat;
    const longitude = event.latlng.lng;
    console.log({latitude,longitude});

    this.layers = [];
    this.layers.push(marker([latitude,longitude],this.markerOptions));
    this.coordinatedSelected.emit({latitude,longitude});

  }
}
