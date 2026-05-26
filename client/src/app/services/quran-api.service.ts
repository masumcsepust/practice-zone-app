import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SurahDto {
  id: number;
  surahNumber: number;
  nameArabic: string;
  nameEnglish: string;
  nameBangla: string;
  totalAyahs: number;
}

export interface AyahDto {
  id: number;
  surahId: number;
  ayahNumber: number;
  arabicText: string;
  englishTranslation: string;
  banglaTranslation: string;
  transliteration: string;
}

@Injectable({ providedIn: 'root' })
export class QuranApiService {
  private readonly http = inject(HttpClient);
  private readonly base = 'http://localhost:5092/api/quran';

  getSurahs(): Observable<SurahDto[]> {
    return this.http.get<SurahDto[]>(`${this.base}/surahs`);
  }

  getAyahs(surahId: number): Observable<AyahDto[]> {
    return this.http.get<AyahDto[]>(`${this.base}/surahs/${surahId}/ayahs`);
  }
}
