import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { GenresService } from '../genres.service';
import { GenreDTO } from '../genres.models';
import { environment } from '../../../environments/environment.development';
import { MatTableModule } from '@angular/material/table';
import { GenericListComponent } from "../../shared/components/generic-list/generic-list.component";
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { PaginationDTO } from '../../shared/models/PaginationDTO';
import { HttpResponse } from '@angular/common/http';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-index-genres',
  imports: [RouterLink, MatIconModule, MatButtonModule, MatTableModule, GenericListComponent,
    MatPaginatorModule,SweetAlert2Module
  ],
  templateUrl: './index-genres.component.html',
  styleUrl: './index-genres.component.css'
})
export class IndexGenresComponent {

  genresService = inject(GenresService);
  genres!:GenreDTO[];
  columnsToDisplay=['id','name','actions'];
  pagination:PaginationDTO ={page: 1,recordsperPage:5};
  totalRecordsCount!: number;

  constructor(){
    this.loadRecords();
  }
  loadRecords(){
    this.genresService.getPaginated(this.pagination).subscribe((response:HttpResponse<GenreDTO[]>) =>{
      this.genres = response.body as GenreDTO[];
      const header = response.headers.get('total-records-count') as string;
      this.totalRecordsCount = parseInt(header,10);
    })
  }
  updatePagination(data:PageEvent)
  {
this.pagination ={page:data.pageIndex + 1, recordsperPage:data.pageSize};
this.loadRecords();
  }
  delete(id:number)
  {
    this.genresService.delete(id).subscribe(()=>{
      this.loadRecords();
    })
  }
}
