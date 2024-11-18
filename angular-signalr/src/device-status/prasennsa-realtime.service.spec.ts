import { TestBed } from '@angular/core/testing';

import { PrasennsaRealtimeService } from './prasennsa-realtime.service';

describe('PrasennsaRealtimeService', () => {
  let service: PrasennsaRealtimeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PrasennsaRealtimeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
