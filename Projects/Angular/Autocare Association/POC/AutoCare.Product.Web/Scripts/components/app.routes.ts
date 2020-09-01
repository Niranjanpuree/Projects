import { Routes, RouterModule } from '@angular/router';
import { VehicleRoutes } from './vehicle/vehicle.routes';
import { ReferenceDataRoutes } from './referenceData/referenceData.routes';
import { PCADBReferenceDataRoutes } from './PCADB/referenceData/referenceData.routes';
import { QDBReferenceDataRoutes } from './QDB/referenceData/referenceData.routes';
import { BaseVehicleRoutes } from './baseVehicle/baseVehicle.routes';
import { BrakeConfigRoutes } from './brakeConfig/brakeConfig.routes';
import { VehicleToBrakeConfigRoutes } from './vehicleToBrakeConfig/vehicleToBrakeConfig.routes';
import { VehicleToBedConfigRoutes } from './vehicleToBedConfig/vehicleToBedConfig.routes';
import { VehicleToBodyStyleConfigRoutes } from './vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.routes';
import { VehicleToWheelBaseRoutes } from './vehicleToWheelBase/vehicleToWheelBase.routes';
import { ChangeRoutes } from './change/change.routes';
import { DashboardRoutes } from './dashboard/dashboard.routes';
import { SystemRoutes } from "./system/system.routes";
import { BedConfigRoutes} from './bedConfig/bedConfig.routes';
import { BodyStyleConfigRoutes} from './bodyStyleConfig/bodyStyleConfig.routes';
import { DriveTypeRoutes} from './driveType/driveType.routes';
import { MfrBodyCodeRoutes} from './mfrBodyCode/mfrBodyCode.routes';
import { WheelBaseRoutes} from './wheelBase/wheelBase.routes';
import { VehicleToMfrBodyCodeRoutes} from './vehicleToMfrBodyCode/vehicleToMfrBodyCode.routes';
import { VehicleToDriveTypeRoutes } from './vehicleToDriveType/vehicleToDriveType.routes';


export const AppRoutes: Routes = [
    ...DashboardRoutes,
    ...ReferenceDataRoutes,
    ...BaseVehicleRoutes,
    ...BrakeConfigRoutes,
    ...VehicleRoutes,
    ...VehicleToBrakeConfigRoutes,
    ...VehicleToBodyStyleConfigRoutes,
    ...ChangeRoutes,
    ...SystemRoutes,
    ...PCADBReferenceDataRoutes,
    ...QDBReferenceDataRoutes,
    ...BedConfigRoutes,
    ...VehicleToBedConfigRoutes,
    ...BodyStyleConfigRoutes,
    ...VehicleToWheelBaseRoutes,
    ...DriveTypeRoutes,
    ...VehicleToDriveTypeRoutes,
    ...VehicleToMfrBodyCodeRoutes,
    ...MfrBodyCodeRoutes,
    ...WheelBaseRoutes,
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
]

//TODO: pushkar: other remaining routes, exceptionhandler, toastmanager
export const APP_ROUTE_PROVIDERS = RouterModule.forRoot(AppRoutes);
