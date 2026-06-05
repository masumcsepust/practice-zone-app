import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

export interface SyllableSoundDto {
  id: string;
  letterId: string;
  letterCharacter: string;
  letterNameEn: string;
  signId: string;
  signSymbol: string;
  signNameEn: string;
  combinedCharacter: string;
  transliterationEn: string;
  transliterationBn: string;
  audioUrl: string;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

export interface CreateSyllableSoundDto {
  letterId: string;
  signId: string;
  combinedCharacter: string;
  transliterationEn: string;
  transliterationBn: string;
  audioUrl: string;
}

export interface UpdateSyllableSoundDto {
  combinedCharacter: string;
  transliterationEn: string;
  transliterationBn: string;
  audioUrl: string;
}

@Injectable({ providedIn: 'root' })
export class SyllableSoundService {
  private readonly http = inject(HttpClient);
  private readonly base = 'http://localhost:5092/api/syllable-sounds';

  getPage(page = 1, pageSize = 50): Observable<PagedResult<SyllableSoundDto>> {
    return this.http.get<PagedResult<SyllableSoundDto>>(
      `${this.base}?page=${page}&pageSize=${pageSize}`
    );
  }

  getAll(): Observable<SyllableSoundDto[]> {
    return this.getPage(1, 200).pipe(map(r => r.items));
  }

  getById(id: string): Observable<SyllableSoundDto> {
    return this.http.get<SyllableSoundDto>(`${this.base}/${id}`);
  }

  create(dto: CreateSyllableSoundDto): Observable<SyllableSoundDto> {
    return this.http.post<SyllableSoundDto>(this.base, dto);
  }

  update(id: string, dto: UpdateSyllableSoundDto): Observable<SyllableSoundDto> {
    return this.http.put<SyllableSoundDto>(`${this.base}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
