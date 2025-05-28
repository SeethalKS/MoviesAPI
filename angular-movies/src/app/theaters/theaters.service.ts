import { inject, Injectable } from '@angular/core';
import { ICRUDService } from '../shared/interfaces/ICURDService';
import { TheatreCreationDTO, TheatreDTO } from './theaters.models';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginationDTO } from '../shared/models/PaginationDTO';
import { environment } from '../../environments/environment.development';
import { buildQueryParams } from '../shared/functions/buildQueryParams';

@Injectable({
  providedIn: 'root'
})
export class TheatersService implements ICRUDService <TheatreDTO,TheatreCreationDTO>{

  constructor() { }

  private http = inject(HttpClient);
  private baseURL = environment.apiURL + '/theaters';

  getPaginated(pagination: PaginationDTO): Observable<HttpResponse<TheatreDTO[]>> {
    let queryParams = buildQueryParams(pagination);
    return this.http.get<TheatreDTO[]>(this.baseURL,{params:queryParams,observe:'response'});

  }
  getById(id: number): Observable<TheatreDTO> {
    return this.http.get<TheatreDTO>(`${this.baseURL}/${id}`);
  }
  create(entity: TheatreCreationDTO): Observable<any> {
    return this.http.post(this.baseURL,entity);
  }
  update(id: number, entity: TheatreCreationDTO): Observable<any> {
    return this.http.put(`${this.baseURL}/${id}`,entity);
  }
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseURL}/${id}`);
  }
}
