import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { GenericListComponent } from '../../shared/components/generic-list/generic-list.component';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ActorsService } from '../actors.service';
import { PaginationDTO } from '../../shared/models/PaginationDTO';
import { ActorDTO } from '../actors.models';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-index-actors',
  imports: [MatButtonModule,MatIconModule,RouterLink,MatTableModule,GenericListComponent,MatPaginatorModule],
  templateUrl: './index-actors.component.html',
  styleUrl: './index-actors.component.css'
})
export class IndexActorsComponent {
actorsService = inject(ActorsService);
pagination:PaginationDTO ={page:1,recordsperPage:5};
totalRecordsCount!:number;
columnsToDisplay=['id','name','actions'];
actors!: ActorDTO[];

loadRecords(){
  this.actorsService.getPaginated(this.pagination).subscribe((response:HttpResponse<ActorDTO[]>)=>{
    this.actors = response.body as ActorDTO[];
    const header = response.headers.get('total-records-count') as string;
    this.totalRecordsCount = parseInt(header,10);
  });
}
updatePagination(data:PageEvent)
{
  this.pagination ={page:data.pageIndex +1,recordsperPage:data.pageSize};
  this.loadRecords();
}
}
