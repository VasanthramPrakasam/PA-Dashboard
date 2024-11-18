import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceStatusComponent } from './device-status.component';

describe('DeviceStatusComponent', () => {
  let component: DeviceStatusComponent;
  let fixture: ComponentFixture<DeviceStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeviceStatusComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
