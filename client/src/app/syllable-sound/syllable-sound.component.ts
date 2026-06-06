import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  SyllableSoundService,
  SyllableSoundDto,
  CreateSyllableSoundDto,
  UpdateSyllableSoundDto
} from '../services/syllable-sound.service';
import { QuranApiService, ArabicLetterDto } from '../services/quran-api.service';
import { HttpClient } from '@angular/common/http';

interface DiacriticSignDto {
  id: string;
  symbol: string;
  nameEn: string;
  nameBn: string;
  signGroup: string;
}

type ModalMode = 'create' | 'edit';

@Component({
  selector: 'app-syllable-sound',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './syllable-sound.component.html',
  styleUrl: './syllable-sound.component.css'
})
export class SyllableSoundComponent implements OnInit {
  private readonly svc    = inject(SyllableSoundService);
  private readonly letApi = inject(QuranApiService);
  private readonly http   = inject(HttpClient);
  private readonly apiBase = 'http://localhost:5092/api';

  sounds   = signal<SyllableSoundDto[]>([]);
  letters  = signal<ArabicLetterDto[]>([]);
  signs    = signal<DiacriticSignDto[]>([]);
  loading  = signal(false);
  error    = signal('');

  showModal  = signal(false);
  modalMode  = signal<ModalMode>('create');
  editId     = signal<string | null>(null);
  saving     = signal(false);
  deleteId   = signal<string | null>(null);

  form = signal<CreateSyllableSoundDto>({
    letterId: '',
    signId: '',
    combinedCharacter: '',
    transliterationEn: '',
    transliterationBn: '',
    transliterationText: '',
    audioUrl: ''
  });

  ngOnInit(): void {
    this.load();
    this.letApi.getLetters(200).subscribe(l => this.letters.set(l));
    this.http.get<DiacriticSignDto[]>(`${this.apiBase}/diacritic-signs`)
      .subscribe(s => this.signs.set(s));
  }

  load(): void {
    this.loading.set(true);
    this.error.set('');
    this.svc.getAll().subscribe({
      next:  items => { this.sounds.set(items); this.loading.set(false); },
      error: err   => { this.error.set(err.message ?? 'Load failed'); this.loading.set(false); }
    });
  }

  openCreate(): void {
    this.form.set({ letterId: '', signId: '', combinedCharacter: '', transliterationEn: '', transliterationBn: '', transliterationText: '', audioUrl: '' });
    this.editId.set(null);
    this.modalMode.set('create');
    this.showModal.set(true);
  }

  openEdit(s: SyllableSoundDto): void {
    this.form.set({
      letterId:           s.letterId,
      signId:             s.signId,
      combinedCharacter:  s.combinedCharacter,
      transliterationEn:  s.transliterationEn,
      transliterationBn:  s.transliterationBn,
      transliterationText: s.transliterationText,
      audioUrl:           s.audioUrl
    });
    this.editId.set(s.id);
    this.modalMode.set('edit');
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.editId.set(null);
  }

  save(): void {
    this.saving.set(true);
    const f = this.form();

    if (this.modalMode() === 'create') {
      this.svc.create(f).subscribe({
        next: () => { this.saving.set(false); this.closeModal(); this.load(); },
        error: err => { this.saving.set(false); this.error.set(err.error?.detail ?? err.message ?? 'Save failed'); }
      });
    } else {
      const id = this.editId()!;
      const upd: UpdateSyllableSoundDto = {
        combinedCharacter:  f.combinedCharacter,
        transliterationEn:  f.transliterationEn,
        transliterationBn:  f.transliterationBn,
        transliterationText: f.transliterationText,
        audioUrl:           f.audioUrl
      };
      this.svc.update(id, upd).subscribe({
        next: () => { this.saving.set(false); this.closeModal(); this.load(); },
        error: err => { this.saving.set(false); this.error.set(err.error?.detail ?? err.message ?? 'Update failed'); }
      });
    }
  }

  confirmDelete(id: string): void {
    this.deleteId.set(id);
  }

  cancelDelete(): void {
    this.deleteId.set(null);
  }

  doDelete(): void {
    const id = this.deleteId();
    if (!id) return;
    this.svc.delete(id).subscribe({
      next: () => { this.deleteId.set(null); this.load(); },
      error: err => { this.error.set(err.message ?? 'Delete failed'); this.deleteId.set(null); }
    });
  }

  playAudio(url: string): void {
    if (!url) return;
    const src = url.startsWith('http') ? url : `${this.apiBase}${url}`;
    new Audio(src).play().catch(() => {});
  }

  patchForm(partial: Partial<CreateSyllableSoundDto>): void {
    this.form.update(f => ({ ...f, ...partial }));
  }
}
