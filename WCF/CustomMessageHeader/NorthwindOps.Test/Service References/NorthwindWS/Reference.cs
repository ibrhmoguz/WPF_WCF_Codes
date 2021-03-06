﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test.NorthwindWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="NorthwindWS.INorthwindOpsService")]
    public interface INorthwindOpsService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INorthwindOpsService/SuppliersByCountry", ReplyAction="http://tempuri.org/INorthwindOpsService/SuppliersByCountryResponse")]
        NorthwindOps.Types.Suppliers[] SuppliersByCountry(string pCountry);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INorthwindOpsService/OrdersByCustomer", ReplyAction="http://tempuri.org/INorthwindOpsService/OrdersByCustomerResponse")]
        NorthwindOps.Types.Orders[] OrdersByCustomer(string pCountry);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INorthwindOpsService/CategoryList", ReplyAction="http://tempuri.org/INorthwindOpsService/CategoryListResponse")]
        NorthwindOps.Types.Categories[] CategoryList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INorthwindOpsService/ProductsByCategory", ReplyAction="http://tempuri.org/INorthwindOpsService/ProductsByCategoryResponse")]
        NorthwindOps.Types.Products[] ProductsByCategory(int pCategoryID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface INorthwindOpsServiceChannel : Test.NorthwindWS.INorthwindOpsService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NorthwindOpsServiceClient : System.ServiceModel.ClientBase<Test.NorthwindWS.INorthwindOpsService>, Test.NorthwindWS.INorthwindOpsService {
        
        public NorthwindOpsServiceClient() {
        }
        
        public NorthwindOpsServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NorthwindOpsServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NorthwindOpsServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NorthwindOpsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public NorthwindOps.Types.Suppliers[] SuppliersByCountry(string pCountry) {
            return base.Channel.SuppliersByCountry(pCountry);
        }
        
        public NorthwindOps.Types.Orders[] OrdersByCustomer(string pCountry) {
            return base.Channel.OrdersByCustomer(pCountry);
        }
        
        public NorthwindOps.Types.Categories[] CategoryList() {
            return base.Channel.CategoryList();
        }
        
        public NorthwindOps.Types.Products[] ProductsByCategory(int pCategoryID) {
            return base.Channel.ProductsByCategory(pCategoryID);
        }
    }
}
