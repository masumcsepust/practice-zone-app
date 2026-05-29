import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

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

export interface ArabicLetterDto {
  id: number;
  order: number;
  letter: string;
  nameEnglish: string;
  nameArabic: string;
  nameBangla: string;
  transliteration: string;
  makhrajType: string;
  makhrajDescription: string;
  sifaat: string[];
  exampleWordArabic: string;
  exampleWord: string;
  audioUrl: string;
}

@Injectable({ providedIn: 'root' })
export class QuranApiService {
  private readonly http = inject(HttpClient);
  private readonly base = 'http://localhost:5092/api';

  getSurahs(): Observable<SurahDto[]> {
    return this.http.get<SurahDto[]>(`${this.base}/quran/surahs`);
  }

  getAyahs(surahId: number): Observable<AyahDto[]> {
    return this.http.get<AyahDto[]>(`${this.base}/quran/surahs/${surahId}/ayahs`);
  }

  getLetters(pageSize = 100): Observable<ArabicLetterDto[]> {
    return this.http
      .get<{ items: ArabicLetterDto[] }>(`${this.base}/arabic-letters?page=1&pageSize=${pageSize}`)
      .pipe(map(r => r.items));
  }
}
