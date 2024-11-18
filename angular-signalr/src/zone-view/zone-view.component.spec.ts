import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ZoneViewComponent } from './zone-view.component';
import {provideRouter, withRouterConfig} from "@angular/router";

describe('ZoneViewComponent', () => {
  let component: ZoneViewComponent;
  let fixture: ComponentFixture<ZoneViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ZoneViewComponent],
      providers:[provideRouter([])]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ZoneViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
