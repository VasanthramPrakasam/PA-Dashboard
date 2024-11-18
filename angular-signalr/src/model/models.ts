export interface Device {
  name?: string;
  isConnected?: ConnectedState;
  fault?: string[];
  status?: status;
  displayName?: string;
  displayFault?: string;
}

export enum ConnectedState {
  Connected, Disconnected, Faulted
}

export enum status {
  green, yellow, red
}

export interface LoginForm {
  userName?: string;
  password?: string;
  ipOrHostName?: string
}

export interface Legend {
  color?: string;
  text?: string;
}

export interface ZoneUse {
  name?: string;
  inUse?: boolean;
}

export interface DeviceFault {
  name?: string;
  fault?: string;
}
