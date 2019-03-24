pipeline {
	agent any
	environment {
		// Very Important; stops from overwriting on temporary files
		HOME = '/tmp'

		// Global Variables
		ApplicationName = "Mango"

		// Color Coding
		JobStartCC = '#ffff00'
		SuccessJobCC = '#7cfc00'
		FailureJobCC = '#ff4500'
		UnstableJobCC = '#0000ff'

		// Slack Notification
		JobStartSN = "STARTED: Job has Started - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Job URL>) - (<${env.BUILD_URL}/console|Console Output>)"
		JobSuccessSN = "SUCCESS: Job - ${currentBuild.fullDisplayName} has been completed. (<${env.BUILD_URL}|Job URL>) - (<${env.BUILD_URL}/console|Console Output>)"
		JobFailureSN = "FAILURE: Job - ${currentBuild.fullDisplayName} has failed. (<${env.BUILD_URL}|Job URL>) - (<${env.BUILD_URL}/console|Console Output>)"
		UnstableJobSN = "UNSTABLE: Job - ${currentBuild.fullDisplayName} is unstable. (<${env.BUILD_URL}|Job URL>) - (<${env.BUILD_URL}/console|Console Output>)"

		// Dotnet
			// Dotnet Bat Variables
			DotnetProjectName = "Mango.sln"
			DotnetProjectOptions = "/p:Configuration=Release /p:Platform=\"Any CPU\""
			DotnetProductVersion = "/p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"

			// Dotnet Run Command Variables
			DotnetBuild = 'nuget restore'
			DotnetTest = 'dotnet test'

			//Nunit
			NunitTest = "nunit3-console"
			NunitResultOutput = "/result:Release/Nunit/Mango-${env.BUILD_NUMBER}-Build-${env.BUILD_NUMBER}/TestReport.xml"

			// Dotnet Test
			DotnetTestResultDir = "-o Release/UnitTest/Mango-${env.BUILD_NUMBER}-Build-${env.BUILD_NUMBER}/TestReport.xml"

			// Dotnet Release
			DotnetReleaseOutputDir = "-o Release/FDD/Mango-${env.BUILD_NUMBER}-Build-${env.BUILD_NUMBER}/"
			DotnetReleaseFDD = "dotnet publish ${env.DotnetProjectName} -c Release ${env.DotnetReleaseOutputDir}"
	}

	stages {
		stage ('Build: Dotnet Project') {
			agent {
				docker { 
					image 'microsoft/dotnet'
				}
			}
			steps {
				slackSend channel: '#bangalore_dev_team',
					color: "${env.JobStartCC}",
					message:  "${env.JobStartSN}"
				sh "dotnet build ${env.DotnetProjectName}"
			}
		}
		stage ('Testing: Nunit Testing') {
			agent {
				docker {
					image 'fela98/mono-nunit'
				}
			}
			steps {
				/*
				* Changed the test command - without project solution name
				*/
				dir ("Mango") {
					sh "${env.NunitTest} Mango.csproj ${env.NunitResultOutput}"
					nunit testResultsPattern: "Release/Nunit/Mango-${env.BUILD_NUMBER}-Build-${env.BUILD_NUMBER}/TestReport.xml"
				}
			}
		}
		stage ('Publish: Dotnet Project FDD & SCD') {
			agent {
				docker { 
					image 'microsoft/dotnet'
				}
			}
			steps {
				sh "${env.DotnetReleaseFDD}"
				sh "tar -czvf mango.tar.gz Mango/Release/*"
				sh "curl -uadmin:AP4ZpfcUDj5N2o7gJ6eP6fqgnui -T mango.tar.gz \"https://dev.celominds.com/artifactory/mango/dotnet-core/${env.JOB_NAME}-${env.BUILD_NUMBER}/mango.tar.gz\""
			}
		}
		stage ('Jfrog Artifactory: Upload') {
			steps {
				sh "curl -uadmin:AP4ZpfcUDj5N2o7gJ6eP6fqgnui -T mangodb.sql \"https://dev.celominds.com/artifactory/mango/database/${env.JOB_NAME}-${env.BUILD_NUMBER}/mangodb.sql\""
			}
		}
		stage ('Jfrog Artifactory: Download') {
			steps {
				sh "cd /home/Artifactory/mango | curl -uadmin:AP4ZpfcUDj5N2o7gJ6eP6fqgnui -O \"https://dev.celominds.com/artifactory/mango/database/${env.JOB_NAME}-${env.BUILD_NUMBER}/mangodb.sql\""
				sh "cd /home/Artifactory/mango | curl -uadmin:AP4ZpfcUDj5N2o7gJ6eP6fqgnui -O \"https://dev.celominds.com/artifactory/mango/dotnet-core/${env.JOB_NAME}-${env.BUILD_NUMBER}/mango.tar.gz\""
				sh "cd /home/Artifactory/mango | tar -xvzf mango.tar.gz"
			}
		}

		/*
		*
		* Archive artifactory is removed as we are managing our binaries in jFrog
		*
		*/

		/*
		* Deployment will be tried after the above steps are successfull
		*
		stage ('Docker: Clearing Running Containers') {
			environment {
				containerId = sh(script: "docker ps --quiet --filter name=sql-supernet mango", returnStdout: true).trim()
			}
			when {
				expression {
					return containerId.isEmpty()
				}
			}
			steps {
				steps {
					sh "docker rm sql-supernet mango"
				}
			}
		}
		stage ('Deployment: SQL Server') {
			steps {
				sh "docker run --name sql-supernet -e \'ACCEPT_EULA=Y\' -e \'SA_PASSWORD=microIn@23\' -e \'MSSQL_PID=Express\' -p 8600:1433 -v /home/Artifactory/\"mango\"/:/transfer -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu"
				sh "docker exec -d sql-supernet /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P \'microIn@23\' -Q \'CREATE DATABASE Supernet\'"
				sh "docker exec -d sql-supernet /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P \'microIn@23\' -Q -i /transfer/mangodb.sql"
			}
		}
		stage ('Deployment: ASP.CORE Application') {
			steps {
				sh "docker run -d --name mango -v /home/Artifactory/\"mango\"/:/transfer -p 8700:50620 --link sql-supernet:sql-supernet -d microsoft/dotnet"
			}
		}
		*
		*
		*/
	}
	post {
		success {
			slackSend channel: '#bangalore_dev_team',
					color: "${env.SuccessJobCC}",
					message: JobSuccessSN
		}
		failure {
			slackSend channel: '#bangalore_dev_team',
					color: "${env.FailureJobCC}",
					message: JobFailureSN
		}
		unstable {
			slackSend channel: '#bangalore_dev_team',
					color: "${env.UnstableJobCC}",
					message: UnstableJobSN
		}
	}
}