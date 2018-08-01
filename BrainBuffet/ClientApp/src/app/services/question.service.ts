import { Injectable } from '@angular/core';
import { throwError as observableThrowError, Observable, of, pipe } from 'rxjs';
import { HttpClient, HttpHeaders, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { map, tap, catchError } from 'rxjs/operators';
import { Question } from '../models/Question';


@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  private headers: HttpHeaders;

  constructor(private _httpClient: HttpClient) {
    this.headers = new HttpHeaders({
      'content-type': 'application/json',
      'Cache-Control': 'no-cache',
      'Pragma': 'no-cache',
      'Expires': 'Sat, 01 Jan 2000 00:00:00 GMT'
    });
  }

  public getRandomList(howmany: number): Observable<number[]> {
    let options = { headers: this.headers };
    var url = `api/questions/randomlist/${howmany}`
    return this._httpClient.get<number[]>(url, options)
      .pipe(
        catchError(err => this.handleError(err))
      )
  }

  public getRandomQuestion(): Observable<Question> {
    let options = { headers: this.headers };
    var url = 'api/questions/random';
    return this._httpClient.get<Question>(url, options)
      .pipe(
        catchError(err => this.handleError(err))
      )
  }

  public getQuestionById(id: number): Observable<Question> {
    let options = { headers: this.headers };
    var url = `api/questions/${id}`;
    return this._httpClient.get<Question>(url, options)
      .pipe(
        catchError(err => this.handleError(err))
      )
  }


  private handleError(error: HttpErrorResponse) {
    // In a real world app, you might use a remote logging infrastructure
    let errMsg: string;

    console.log(error);
    if (error.status == 401) {
      errMsg = 'Invalid Username or Password'
    }
    else { errMsg = 'Error: ' + error.error };

    console.log(errMsg);
    return observableThrowError(errMsg);
  }
}
